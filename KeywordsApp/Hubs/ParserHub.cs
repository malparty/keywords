using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace KeywordsApp.Hubs
{
    public class ParserHub : Hub
    {
        public async Task SendStatusUpdate(string username, int keywordId, string status)
        {
            await Clients.User(username).SendAsync("ReceiveStatusUpdate", keywordId, status);
        }
    }
}