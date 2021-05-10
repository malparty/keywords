using System;
using System.ComponentModel.DataAnnotations.Schema;
using KeywordsApp.Models.File;

namespace KeywordsApp.Models.Keyword
{
    [Table("Keywords")]
    public class KeywordEntity
    {
        public KeywordEntity() { }
        public KeywordEntity(string name)
        {
            Name = name;
            ParsingStatus = ParsingStatus.Pending;
        }
        public int Id { get; set; }

        [Column(TypeName = "character varying(200)")]
        public string Name { get; set; }
        public DateTime? ParsedDate { get; set; }
        public ParsingStatus ParsingStatus { get; set; }

        #region Keyword Results
        public int AdWordsCount { get; set; }
        public int LinkCount { get; set; }

        public double TotalThouthandResultsCount { get; set; }
        public string HtmlCode { get; set; }

        // Miliseconds
        public int RequestDuration { get; set; }
        #endregion
        public int FileId { get; set; }
        public FileEntity File { get; set; }
    }
}
