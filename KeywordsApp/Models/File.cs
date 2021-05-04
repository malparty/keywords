using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KeywordsApp.Models
{

    [Table("Users")]
    [Index(nameof(CreatedDate), nameof(Name))]
    public class CsvFile
    {
        public int Id { get; set; }

        [Column(TypeName = "character varying(256)")]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public IEnumerable<Keyword> Keywords { get; set; }

    }
}