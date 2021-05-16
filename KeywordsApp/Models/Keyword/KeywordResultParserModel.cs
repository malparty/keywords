namespace KeywordsApp.Models.Keyword
{
    public class KeywordResultParserModel
    {
        public int AdWordsCount { get; set; }
        public int LinkCount { get; set; }
        public int RequestDuration { get; set; }

        public double TotalThouthandResultsCount { get; set; }

        public string HtmlCode { get; set; }
        public string ErrorMsg { get; set; }

        public bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(ErrorMsg);
            }
        }
    }
}
