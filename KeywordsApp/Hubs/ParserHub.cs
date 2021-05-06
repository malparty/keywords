using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace KeywordsApp.Hubs
{
    public class ParserHub : Hub<IParser>
    {
        // Event from outside
        public async Task SendStatusUpdate(string username, int keywordId, string status)
        {
            Console.WriteLine("ParserHUB WORKING {0}, {1}, {2}", username, keywordId, status);
            await Clients.User(username).KeywordStatusUpdate("ReceiveStatusUpdate", keywordId, status);
        }
    }
}