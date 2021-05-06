using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace KeywordsApp.Hubs
{
    public class ParserHub : Hub
    {
        public async Task SendStatusUpdate(int keywordId, string message)
        {
            await Clients.All.SendAsync("ReceiveStatusUpdate", keywordId, message);
        }
    }
}