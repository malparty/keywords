using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using KeywordsApp.Models.Keyword;

namespace KeywordsApp.Models.File
{

    [Table("CsvFile")]
    [Index(nameof(CreatedDate), nameof(Name))]
    public class FileEntity
    {
        public int Id { get; set; }

        [Column(TypeName = "character varying(256)")]
        public string Name { get; set; }
        public UserEntity CreatedByUser { get; set; }

        public DateTime CreatedDate { get; set; }

        public IEnumerable<KeywordEntity> Keywords { get; set; }

    }
}