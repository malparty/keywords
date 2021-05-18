using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace KeywordsApp.HostedServices
{
    // Perform Http requests to Google
    public class HttpRequestService : IHttpRequestService
    {
        private const string googleUrl = "https://www.google.com/search";
        private const int MAX_RETRY = 5;
        private const int DELAY_BEFORE_RETRY = 5000;

        private int _retryCount;
        private bool _isSucceed;
        private HttpClient _client;


        private readonly ILogger<HttpRequestService> _logger;


        public HttpRequestService(ILogger<HttpRequestService> logger)
        {
            _logger = logger;
            _retryCount = 0;
            _isSucceed = false;
        }


        public async Task<string> QueryHtmlContentAsync(string name, int keywordId)
        {
            _client = new HttpClient();
            string result = null;


            // Retry approach:
            while (_retryCount < MAX_RETRY && !_isSucceed)
            {
                result = await TryQueryHtmlContentAsync(name, keywordId);
            }

            _client.Dispose();

            return result;
        }

        public async Task<string> TryQueryHtmlContentAsync(string name, int keywordId)
        {
            try
            {
                // Build request:
                var request = BuildRequest(name);

                // Send request
                var response = await _client.SendAsync(request, cancellationToken: CancellationToken.None);

                if (response.IsSuccessStatusCode)
                {
                    // Read Html content
                    var result = await response.Content.ReadAsStringAsync();
                    _isSucceed = true;

                    return result;  
                }
                // Basic Retry (no delay) with HttpClient disposal
                else
                {
                    await HandleRequestBlockedAsync(false);
                }
            }
            catch (HttpRequestException reqExcep)
            {
                await HandleHttpRequestExceptionAsync(reqExcep);
            }
            catch (Exception e)
            {
                if (e.InnerException.GetType() == typeof(HttpRequestException))
                {
                    await HandleHttpRequestExceptionAsync((HttpRequestException)e.InnerException);
                }
                else
                {
                    var errorMsg = string.Format("Unknown exception occur for keyword {0}", keywordId);
                    _logger.LogError(0, errorMsg, e);
                    _logger.LogError(0, e.Message);
                }
            }
            return null;
        }


        private async Task HandleHttpRequestExceptionAsync(HttpRequestException e)
        {
            _logger.LogError(0, "HttpRequestException catched. Trying to HandleRequestBlockedAsync with delay.", e);
            await HandleRequestBlockedAsync(true);
        }

        private async Task HandleRequestBlockedAsync(bool withDelay)
        {
            _client.Dispose();

            if (withDelay)
                await Task.Delay(DELAY_BEFORE_RETRY);

            _client = new HttpClient();
            _retryCount++;
        }

        private HttpRequestMessage BuildRequest(string name)
        {
            var queryGoogle = googleUrl + "?q=" + name;

            var request = new HttpRequestMessage(HttpMethod.Get, queryGoogle);

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

            return request;
        }
    }
}