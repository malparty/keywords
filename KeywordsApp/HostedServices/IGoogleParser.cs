
namespace KeywordsApp.HostedServices
{
    public interface IGoogleParser
    {
        public void ParseAsync(bool includeFailed = false);
    }
}
