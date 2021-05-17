using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeywordsApp.Data;
using KeywordsApp.Hubs;
using KeywordsApp.Models.Keyword;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KeywordsApp.HostedServices
{
    public class GoogleParser : IGoogleParser
    {
        // Processing pool, enable to know what kw are being processed
        public List<KeywordParserModel> ToBeParsedKeywords { get; set; }

        private readonly IHubContext<ParserHub, IParser> _parserHub;
        private readonly ILogger<ParserService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public GoogleParser(ILogger<ParserService> logger, IHubContext<ParserHub, IParser> parserHub, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _parserHub = parserHub;
            _scopeFactory = scopeFactory;
            ToBeParsedKeywords = new List<KeywordParserModel>();
        }

        public async void ParseAsync(bool includeFailed = true)
        {
            // Only parse when all current parsing are done
            if (ToBeParsedKeywords.Count > 0)
                return;

            // Get what needs to be parsed.
            await GetToBeParsedKeywordsAsync(includeFailed);

            // Parse keywords that are not being parsed
            foreach (var keyword in ToBeParsedKeywords)
            {
                ParseKeywordAsync(keyword);
            }
        }

        private async Task UpdateKeywordStatusAsync(int keywordId, ParsingStatus status)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<KeywordContext>();

                var keyword = await dbContext.Keywords.FindAsync(keywordId);
                if (keyword == null)
                    return;
                keyword.ParsingStatus = status;
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task SendNotificationAsync(KeywordParserModel keyword, string errorMsg)
        {
            // Get file progress %
            int? percent;
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<KeywordContext>();
                percent = dbContext.Files.Where(x => x.Id == keyword.FileId)
                    .Select(
                        x => (
                            100 * (
                                x.Keywords.Where(k => k.ParsingStatus == ParsingStatus.Succeed).Count()
                            )
                        ) / x.Keywords.Count()
                    )
                    .FirstOrDefault();
            }

            var status = string.IsNullOrEmpty(errorMsg) ? "success" : "fail";
            await _parserHub.Clients.User(keyword.UserId)
                .KeywordStatusUpdate(keyword.FileId, percent ?? 0, keyword.KeywordId, keyword.Name, status, errorMsg);
        }

        private async Task ParseKeywordAsync(KeywordParserModel keyword)
        {
            await UpdateKeywordStatusAsync(keyword.KeywordId, ParsingStatus.Parsing);
            _logger.LogInformation(string.Format("Parsing start for: {0}", keyword.Name));

            var errorMsg = "";
            try
            {
                // Get Html Content
                using (var scope = _scopeFactory.CreateScope())
                {
                    var requestService = scope.ServiceProvider.GetRequiredService<IHttpRequestService>();
                    keyword.RawHtmlContent = await requestService.QueryHtmlContentAsync(keyword.Name, keyword.KeywordId);
                }

                if (string.IsNullOrEmpty(keyword.RawHtmlContent))
                {
                    await UpdateKeywordStatusAsync(keyword.KeywordId, ParsingStatus.Failed);
                    errorMsg = "Could not load HTML content from the search query.";
                    return;
                }
                // Test & Parse Html Content
                var parsingResults = keyword.ParseHtml();
                if (!parsingResults.IsValid)
                {
                    await UpdateKeywordStatusAsync(keyword.KeywordId, ParsingStatus.Failed);
                    errorMsg = parsingResults.ErrorMsg;
                    return;
                }
                // Update KeywordEntity in DB
                var isPersistSucceed = await PersistParsedKeyword(keyword.KeywordId, parsingResults);
                if (!isPersistSucceed)
                {
                    errorMsg = "Could not store changes in Db while parsing Keyword.";
                }
                else
                {
                    await UpdateKeywordStatusAsync(keyword.KeywordId, ParsingStatus.Succeed);
                    _logger.LogInformation(string.Format("Parsing SUCCESS for: {0}", keyword.Name));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(1, string.Format("While parsing keyword {0}", keyword.KeywordId), e);
                // Mark current Keyword FAILED
                await UpdateKeywordStatusAsync(keyword.KeywordId, ParsingStatus.Failed);
            }
            finally
            {
                // Remove current keyword from the processing pool
                ToBeParsedKeywords.Remove(keyword);
                SendNotificationAsync(keyword, errorMsg);
            }
        }

        private async Task GetToBeParsedKeywordsAsync(bool includeFailed = false)
        {
            // Add new keywords from Db
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<KeywordContext>();

                // All Pending/Parsing/Failed
                var query = dbContext.Keywords
                    .Where(
                        x => x.ParsingStatus == ParsingStatus.Pending
                            || x.ParsingStatus == ParsingStatus.Parsing
                            || (
                                includeFailed
                                    ? x.ParsingStatus == ParsingStatus.Failed
                                    : false
                            )
                    );

                ToBeParsedKeywords = await query
                .OrderBy(x => x.ParsingStatus)
                .Select(
                    x => new KeywordParserModel
                    {
                        KeywordId = x.Id,
                        Name = x.Name,
                        UserId = x.File.CreatedByUserId,
                        FileId = x.FileId
                    }
                )
                .ToListAsync();
            }
        }

        private async Task<bool> PersistParsedKeyword(int keywordId, KeywordResultParserModel parsedModel)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<KeywordContext>();
                    var keyword = await dbContext.Keywords.FindAsync(keywordId);
                    keyword.HtmlCode = parsedModel.HtmlCode;
                    keyword.AdWordsCount = parsedModel.AdWordsCount;
                    keyword.LinkCount = parsedModel.LinkCount;
                    keyword.RequestDuration = parsedModel.RequestDuration;
                    keyword.TotalThouthandResultsCount = parsedModel.TotalThouthandResultsCount;
                    keyword.ParsingStatus = ParsingStatus.Succeed;
                    keyword.ParsedDate = DateTime.Now;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(0, string.Format("Error while persisting keyword {0}", keywordId), e);
                return false;
            }
            return true;
        }

    }

}