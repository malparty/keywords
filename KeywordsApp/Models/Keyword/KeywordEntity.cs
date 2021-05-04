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
        }
        public int Id { get; set; }

        [Column(TypeName = "character varying(200)")]
        public string Name { get; set; }

        public int FileId { get; set; }
        public FileEntity File { get; set; }
    }
}
