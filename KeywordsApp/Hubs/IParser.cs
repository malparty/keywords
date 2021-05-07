
using System.Threading.Tasks;

namespace KeywordsApp.Hubs
{
    public interface IParser
    {
        Task KeywordStatusUpdate(int fileId, int keywordId, string keywordName, string status, string errorMsg);
    }

}