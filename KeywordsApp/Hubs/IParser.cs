
using System.Threading.Tasks;

namespace KeywordsApp.Hubs
{
    public interface IParser
    {
        Task KeywordStatusUpdate(string username, int keywordId, string status);
        Task SayHello();
    }

}