using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using KeywordsApp.Data;
using KeywordsApp.Hubs;
using KeywordsApp.Models.Keyword;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KeywordsApp.HostedServices
{
    public class GoogleParser : IGoogleParser
    {
        public bool IsParsing { get; set; }
        private readonly IHubContext<ParserHub, IParser> _parserHub;
        private readonly ILogger<ParserService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;


        public GoogleParser(ILogger<ParserService> logger, IHubContext<ParserHub, IParser> parserHub, IServiceScopeFactory scopeFactory)
        {
            IsParsing = false;
            _logger = logger;
            _parserHub = parserHub;
            _scopeFactory = scopeFactory;
        }

        public async Task ParseAsync(bool includeFailed = false)
        {
            var currentKeywordId = 0;
            try
            {
                IsParsing = true;
                // Check if anything to parse.
                var pendingKeywords = GetPendingKeywords(includeFailed);
                // Parse 1x keyword after another (#User Segregation)
                foreach (var keyword in pendingKeywords)
                {
                    currentKeywordId = keyword.KeywordId;
                    var errorMsg = await ParseKeyword(keyword);
                    await SendNotification(keyword, errorMsg);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(0, string.Format("Cannot parse keyword {0}", currentKeywordId), e);

            }
            finally
            {
                IsParsing = false;
            }
        }
        private async Task MarkKeywordAsFailedAsync(int keywordId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<KeywordContext>();

                var failedKeyword = await dbContext.Keywords.FindAsync(keywordId);
                if (failedKeyword == null)
                    return;
                failedKeyword.ParsingStatus = ParsingStatus.Failed;
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task SendNotification(KeywordParserModel keyword, string errorMsg)
        {
            // Get file progress %
            int? percent;
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<KeywordContext>();
                percent = dbContext.Files.Where(x => x.Id == keyword.FileId)
                    .Select(x =>
                        (100 * (x.Keywords.Where(k => k.ParsingStatus == ParsingStatus.Succeed).Count())) / x.Keywords.Count())
                        .FirstOrDefault();
            }

            var status = string.IsNullOrEmpty(errorMsg) ? "success" : "fail";
            await _parserHub.Clients.User(keyword.UserId).KeywordStatusUpdate(keyword.FileId, percent ?? 0, keyword.KeywordId, keyword.Name, status, errorMsg);
        }

        private async Task<string> ParseKeyword(KeywordParserModel keyword)
        {
            try
            {
                // Get Html Content
                keyword.RawHtmlContent = await QueryHtmlContentAsync(keyword);
                if (string.IsNullOrEmpty(keyword.RawHtmlContent))
                {
                    await MarkKeywordAsFailedAsync(keyword.KeywordId);
                    return "Could not load HTML content from the search query.";
                }
                // Test & Parse Html Content
                var parsingResults = keyword.ParseHtml();
                if (!parsingResults.IsValid)
                {
                    await MarkKeywordAsFailedAsync(keyword.KeywordId);
                    return parsingResults.ErrorMsg;
                }
                // Update KeywordEntity in DB
                var isPersistSucceed = await PersistParsedKeyword(keyword.KeywordId, parsingResults);
                if (!isPersistSucceed)
                {
                    return "Could not store changes in Db while parsing Keyword.";
                }
            }
            catch (Exception e)
            {
                _logger.LogError(1, string.Format("While parsing keyword {0}", keyword.KeywordId), e);
                // Mark current Keyword FAILED
                await MarkKeywordAsFailedAsync(keyword.KeywordId);
            }
            return null;
        }

        // IMPROVEMENT: can have many threads using adapter proxy url and mapping query (post/get, input ids, ...)
        private async Task<string> QueryHtmlContentAsync(KeywordParserModel keyword)
        {
            const int MAX_RETRY = 5;
            HttpClient client = new HttpClient();
            string result = null;
            try
            {
                // Call proxy with Server arg and Google query:
                const string googleUrl = "https://www.google.com/search";
                var queryGoogle = googleUrl + "?q=" + keyword.Name;

                int retryCount = 0;
                HttpResponseMessage response = null;

                // Retry approach:
                do
                {
                    // Build request:
                    var request = new HttpRequestMessage(HttpMethod.Get, queryGoogle);
                    // var request = new HttpRequestMessage(HttpMethod.Post, proxyUrl);

                    // Build Client header
                    request.Headers.Accept.Clear();
                    // The user agent describe to google which device is requesting.
                    // It enable us to get Desktop-style UI instead of Mobile version (that does not include stats)
                    request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    request.Headers.UserAgent.ParseAdd("AppleWebKit/537.36 (KHTML, like Gecko)");
                    request.Headers.UserAgent.ParseAdd("Chrome/90.0.4430.85");
                    request.Headers.UserAgent.ParseAdd("Safari/537.36");
                    request.Headers.UserAgent.ParseAdd("Edg/90.0.818.49");

                    // Add an Accept header for JSON format.
                    // These correspond to what a classic web browser would expose
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("image/apng"));
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/signed-exchange"));

                    // Set language to english
                    request.Headers.AcceptLanguage.ParseAdd("en-US");

                    response = client.SendAsync(request, cancellationToken: CancellationToken.None).Result;

                    // Preventing error 429 - Too many requests
                    if (!response.IsSuccessStatusCode)
                    {
                        retryCount++;
                        _logger.LogInformation(string.Format("Trying to dispose HttpClient (x{0})", retryCount));
                        client.Dispose();
                        client = new HttpClient();
                    }
                } // Exit loop with Successful request or Max Retry count reached.
                while (retryCount < MAX_RETRY && !response.IsSuccessStatusCode);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
                // Too many requests - bot detected
                else if ((int)response.StatusCode == 429)
                {
                    var errorMsg = string.Format("Response 429 (Too many requests) for keyword {0}", keyword.KeywordId);
                    _logger.LogWarning(errorMsg);
                }
                // Unknown error, logged as error
                else
                {
                    var errorMsg = string.Format("Response {0} ({1}) for keyword {2}", response.StatusCode, response.ReasonPhrase, keyword.KeywordId);
                    _logger.LogError(errorMsg);
                }

            }
            catch (Exception e)
            {

                var errorMsg = string.Format("Unknown exception occur for keyword {0}", keyword.KeywordId);
                _logger.LogError(0, errorMsg, e);
                _logger.LogError(0, e.Message);
            }
            finally
            {
                client.Dispose();
            }
            return result;
        }

        private List<KeywordParserModel> GetPendingKeywords(bool includeFailed = false)
        {
            List<KeywordParserModel> pendingKeywords;
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<KeywordContext>();
                var query = dbContext.Keywords.Where(x => x.ParsingStatus == ParsingStatus.Pending
                || (includeFailed
                    ? x.ParsingStatus == ParsingStatus.Failed
                    : false));
                pendingKeywords = query.Select(x => new KeywordParserModel
                {
                    KeywordId = x.Id,
                    Name = x.Name,
                    UserId = x.File.CreatedByUserId,
                    FileId = x.FileId
                }).ToList();
            }
            return pendingKeywords;
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