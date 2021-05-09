using Microsoft.AspNetCore.SignalR;

namespace KeywordsApp.Hubs
{
    // This class is empty because the server does not 
    // need to receive any call from the clients.
    // Though we need it to send calls to our clients
    public class ParserHub : Hub<IParser>
    {
    }
}