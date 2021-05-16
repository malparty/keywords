using System;
using System.ComponentModel.DataAnnotations.Schema;
using KeywordsApp.Models.File;

namespace KeywordsApp.Models.Keyword
{
    [Table("Keywords")]
    public class KeywordEntity
    {
        public int Id { get; set; }

        public int AdWordsCount { get; set; }
        public int LinkCount { get; set; }
        public int RequestDuration { get; set; }
        public double TotalThouthandResultsCount { get; set; }
        [Column(TypeName = "character varying(200)")]

        public string Name { get; set; }
        public string HtmlCode { get; set; }

        public ParsingStatus ParsingStatus { get; set; }
        public DateTime? ParsedDate { get; set; }

        // File Entity        
        public int FileId { get; set; }
        public FileEntity File { get; set; }

        public KeywordEntity() { }

        public KeywordEntity(string name)
        {
            Name = name;
            ParsingStatus = ParsingStatus.Pending;
        }
    }
}
