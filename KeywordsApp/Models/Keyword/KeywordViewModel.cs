using X.PagedList;

namespace KeywordsApp.Models.Keyword
{

    public class KeywordViewModel
    {
        public KeywordOrderBy OrderBy { get; set; }
        public IPagedList<KeywordEntity> Keywords { get; set; }
    }
    public enum KeywordOrderBy
    {
        NameAsc = 1,
        NameDesc = -1,
        Status = 0
    }
}