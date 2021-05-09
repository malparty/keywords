using System;

namespace KeywordsApp.Models.File
{
    public class FileViewModel
    {
        public int FileId { get; set; }
        public string Name { get; set; }
        public bool ShowProgressBar { get; set; }

        public DateTime CreatedDate { get; set; }
        public int TotalKeywordsCount { get; set; }
        public int ParsedKeywordsCount { get; set; }

        public int ParsedPercent
        {
            get
            {
                if (TotalKeywordsCount <= 0)
                    return 0;
                return (100 * ParsedKeywordsCount) / TotalKeywordsCount;
            }
        }
    }
}