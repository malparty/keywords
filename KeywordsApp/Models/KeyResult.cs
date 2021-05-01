using HtmlAgilityPack;

namespace KeywordsApp.Models
{

    public class KeyResult
    {
        public string Keyword { get; set; }
        public string PageContent
        {
            set
            {
                _pageDocument = new HtmlDocument();
                _pageDocument.LoadHtml(value);
            }
        }
        private HtmlDocument _pageDocument;
        public string SearchStats
        {
            get
            {
                return _pageDocument == null ? "-page document null" : _pageDocument.DocumentNode.SelectSingleNode("//div[contains(@id,'result-stats')][1]")?.InnerHtml;
            }
        }
    }
}