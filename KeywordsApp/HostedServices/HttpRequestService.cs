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


        private readonly ILogger<HttpRequestService> _logger;


        public HttpRequestService(ILogger<HttpRequestService> logger)
        {
            _logger = logger;
        }


        public async Task<string> QueryHtmlContentAsync(string name, int keywordId)
        {
            const int MAX_RETRY = 5;
            HttpClient client = new HttpClient();
            string result = null;
            try
            {
                int retryCount = 0;
                HttpResponseMessage response = null;

                // Retry approach:
                do
                {
                    // Build request:
                    var request = BuildRequest(name);

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
                    var errorMsg = string.Format("Response 429 (Too many requests) for keyword {0}", keywordId);
                    _logger.LogWarning(errorMsg);
                }
                // Unknown error, logged as error
                else
                {
                    var errorMsg = string.Format("Response {0} ({1}) for keyword {2}", response.StatusCode, response.ReasonPhrase, keywordId);
                    _logger.LogError(errorMsg);
                }

            }
            catch (Exception e)
            {

                var errorMsg = string.Format("Unknown exception occur for keyword {0}", keywordId);
                _logger.LogError(0, errorMsg, e);
                _logger.LogError(0, e.Message);
            }
            finally
            {
                client.Dispose();
            }
            return result;
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

            return request;
        }
    }
}