using System.Collections.Generic;
using System.Linq;
using X.PagedList;
using X.PagedList.Web.Common;

namespace KeywordsApp.Models.Keyword
{

    public class KeywordListViewModel
    {
        public KeywordOrderBy OrderBy { get; set; }
        public int FileId { get; set; }
        public string Search { get; set; }
        public IPagedList<KeywordEntity> Keywords { get; set; }
        public PagedListOptions PageListOptions { get { return new PagedListOptions(); } }

        // based on orderby, group using the right key
        public IEnumerable<IGrouping<string, KeywordEntity>> GroupedByOrderKey
        {
            get
            {
                if (OrderBy == KeywordOrderBy.StatusAsc || OrderBy == KeywordOrderBy.StatusDesc)
                    return Keywords.GroupBy(x => x.ParsingStatus.ToString());
                else
                    return Keywords.GroupBy(x => x.Name.ToLower()[0].ToString());
            }
        }

    }
    public enum KeywordOrderBy
    {
        NameAsc = 1,
        NameDesc = 2,
        StatusAsc = 3,
        StatusDesc = 4
    }
}