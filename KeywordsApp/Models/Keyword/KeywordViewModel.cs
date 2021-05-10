using System;

namespace KeywordsApp.Models.Keyword
{
    public class KeywordViewModel
    {
        public int KeywordId { get; set; }
        public ParsingStatus ParsingStatus { get; set; }
        public DateTime? ParsedDate { get; set; }
        public string Name { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public int AdWordsCount { get; set; }
        public int LinkCount { get; set; }

        public double TotalThouthandResultsCount { get; set; }
        public int RequestDuration { get; set; }
    }

}