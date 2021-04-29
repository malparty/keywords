using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeywordsApp.Models
{
    [Table("Keywords")]
    public class Keyword
    {
        public int Id { get; set; }

        [Column(TypeName = "character varying(200)")]
        public string Name { get; set; }
    }
}
