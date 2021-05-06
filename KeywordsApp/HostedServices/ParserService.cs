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
        private readonly IHubContext<ParserHub, IParser> _parserHub;

        public ParserService(ILogger<ParserService> logger, IHubContext<ParserHub, IParser> parserHub)
        {
            _logger = logger;
            _parserHub = parserHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {Time}", DateTime.Now);
                // WORKING
                // WORKING
                // WORKING
                // WORKING
                await _parserHub.Clients.All.KeywordStatusUpdate("xavier@malparty.fr", 1, "Coucou toi l'amis!"); // TODO MAP!
                await Task.Delay(5000);
            }
        }
    }
}