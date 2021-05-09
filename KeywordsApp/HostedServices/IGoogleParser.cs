using System;
using System.Threading.Tasks;

namespace KeywordsApp.HostedServices
{
    public interface IGoogleParser
    {
        public bool IsParsing { get; set; }
        public Task ParseAsync(bool includeFailed = false);
    }
}