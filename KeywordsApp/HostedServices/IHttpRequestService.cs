using System.Threading.Tasks;

namespace KeywordsApp.HostedServices
{
    // Perform Http requests to Google
    public interface IHttpRequestService
    {
        Task<string> QueryHtmlContentAsync(string name, int keywordId);
    }
}