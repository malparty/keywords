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
        public List<KeywordParserModel> FetchingKeywords { get; set; }

        // We only start to parse keywords not being parsed
        public List<KeywordParserModel> ToBeParsedKeywords
        {
            get
            {
                return FetchingKeywords
                    .Where(x => x.ParsingStatus != ParsingStatus.Parsing)
                    .ToList();
            }
        }
        public IEnumerable<int> FetchingKeywordIds
        {
            get
            {
                return FetchingKeywords.Select(x => x.KeywordId);
            }
        }
        private readonly IHubContext<ParserHub, IParser> _parserHub;
        private readonly ILogger<ParserService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHttpRequestService _requestService;


        public GoogleParser(ILogger<ParserService> logger, IHubContext<ParserHub, IParser> parserHub, IServiceScopeFactory scopeFactory, IHttpRequestService requestService)
        {
            _logger = logger;
            _parserHub = parserHub;
            _scopeFactory = scopeFactory;
            _requestService = requestService;
        }

        public async void ParseAsync(bool includeFailed = true)
        {
            _logger.LogInformation("Google parser running at: {Time}", DateTime.Now);

            // Update what needs to be parsed.
            await UpdateFetchingKeywordsAsync(includeFailed);

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

        private async void SendNotificationAsync(KeywordParserModel keyword, string errorMsg)
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

        private async void ParseKeywordAsync(KeywordParserModel keyword)
        {
            await UpdateKeywordStatusAsync(keyword.KeywordId, ParsingStatus.Parsing);
            _logger.LogInformation(string.Format("Parsing start for: {0}", keyword.Name));

            var errorMsg = "";
            try
            {
                // Get Html Content
                keyword.RawHtmlContent = await _requestService.QueryHtmlContentAsync(keyword.Name, keyword.KeywordId);

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
            }
            catch (Exception e)
            {
                _logger.LogError(1, string.Format("While parsing keyword {0}", keyword.KeywordId), e);
                // Mark current Keyword FAILED
                await UpdateKeywordStatusAsync(keyword.KeywordId, ParsingStatus.Failed);
            }
            finally
            {
                SendNotificationAsync(keyword, errorMsg);
            }
        }

        private async Task UpdateFetchingKeywordsAsync(bool includeFailed = false)
        {
            if (FetchingKeywords == null)
            {
                FetchingKeywords = new List<KeywordParserModel>();
            }
            // Remove parsed keywords
            FetchingKeywords = FetchingKeywords
                .Where(x => x.ParsingStatus != ParsingStatus.Succeed)
                .ToList();
            if (!includeFailed)
            {
                FetchingKeywords = FetchingKeywords
                    .Where(x => x.ParsingStatus != ParsingStatus.Failed)
                    .ToList();
            }

            // Add new keywords from Db
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<KeywordContext>();

                // All Pending/Failed
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
                // Except those already in FetchingKeywords 
                query = query
                    .Where(
                        x => !FetchingKeywords
                            .Select(y => y.KeywordId)
                            .Contains(x.Id)
                    );
                var newKeywords = await query.Select(
                    x => new KeywordParserModel
                    {
                        KeywordId = x.Id,
                        Name = x.Name,
                        UserId = x.File.CreatedByUserId,
                        FileId = x.FileId
                    }
                )
                .ToListAsync();

                FetchingKeywords.AddRange(newKeywords);
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