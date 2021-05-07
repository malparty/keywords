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
        public IPagedList<KeywordEntity> Keywords { get; set; }
        public PagedListRenderOptionsBase PagedListRenderOptions
        {
            get
            {
                return new X.PagedList.Web.Common.PagedListRenderOptionsBase
                {
                    UlElementClasses = new string[] { "pagination" },
                    LiElementClasses = new string[] { "page-item" },
                    ActiveLiElementClass = "active",
                    PageClasses = new string[] { "page-link" }
                };
            }
        }

        // based on orderby, group using the right key
        public IEnumerable<IGrouping<string, KeywordEntity>> GroupedByOrderKey
        {
            get
            {
                if (OrderBy == KeywordOrderBy.Status)
                    return Keywords.GroupBy(x => x.ParsingStatus.ToString());
                else
                    return Keywords.GroupBy(x => x.Name.ToLower()[0].ToString());
            }
        }

    }
    public enum KeywordOrderBy
    {
        NameAsc = 1,
        NameDesc = -1,
        Status = 0
    }
}