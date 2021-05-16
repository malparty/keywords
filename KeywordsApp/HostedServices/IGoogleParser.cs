
namespace KeywordsApp.HostedServices
{
    public interface IGoogleParser
    {
        public bool IsParsing { get; set; }
        // We do not include Failed within the parser as 
        // failed keywords needs manual investigation 
        // from the dev team
        public void ParseAsync(bool includeFailed = false);
    }
}
