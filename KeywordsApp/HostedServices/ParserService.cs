using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KeywordsApp.HostedServices
{
    public class ParserService : BackgroundService
    {
        private readonly IGoogleParser _googleParser;

        public ParserService(IGoogleParser googleParser)
        {
            _googleParser = googleParser;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _googleParser.ParseAsync(false);
                await Task.Delay(5000); // parsing every seconds
            }
        }
    }
}
