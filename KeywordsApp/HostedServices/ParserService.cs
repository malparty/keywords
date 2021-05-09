using System;
using System.Threading;
using System.Threading.Tasks;
using KeywordsApp.Data;
using KeywordsApp.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KeywordsApp.HostedServices
{
    public class ParserService : BackgroundService
    {
        private readonly ILogger<ParserService> _logger;
        private readonly IGoogleParser _googleParser;

        public ParserService(ILogger<ParserService> logger, IGoogleParser googleParser)
        {
            _logger = logger;
            _googleParser = googleParser;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {Time}", DateTime.Now);
                await _googleParser.ParseAsync(true); // TODO - Define when taking failed and when not!
                // No action until current parsing is done
                while (_googleParser.IsParsing)
                {
                    await Task.Delay(1000);
                }
                // Wait 5 seconds until next parsing check
                await Task.Delay(5000);

            }
        }
    }
}