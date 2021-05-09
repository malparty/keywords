using X.PagedList.Web.Common;

namespace KeywordsApp.Models
{
    public class PagedListOptions
    {
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

    }

}