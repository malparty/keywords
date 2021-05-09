using System.Collections.Generic;

namespace KeywordsApp.Models.Keyword
{
    public class KeywordResultParserModel
    {
        public string ErrorMsg { get; set; }
        public bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(ErrorMsg);
            }
        }
        public int AdWordsCount { get; set; }
        public int LinkCount { get; set; }

        public int TotalThouthandResultsCount { get; set; }
        public string HtmlCode { get; set; }

        // Miliseconds
        public int RequestDuration { get; set; }
    }


}