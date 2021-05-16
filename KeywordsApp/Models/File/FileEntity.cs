using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using KeywordsApp.Models.Keyword;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KeywordsApp.Models.File
{
    [Table("Files")]
    [Index(nameof(CreatedDate), nameof(Name))]
    public class FileEntity
    {
        public int Id { get; set; }

        [Column(TypeName = "character varying(256)")]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        // Created By User entity
        [Required]
        public string CreatedByUserId { get; set; }
        public UserEntity CreatedByUser { get; set; }

        // Keywords Entities
        public IEnumerable<KeywordEntity> Keywords { get; set; }

        public FileEntity() { }

        public FileEntity(string userId, List<string> keywords, string fileName)
        {
            CreatedByUserId = userId;
            CreatedDate = DateTime.Now;
            parseKeywords(keywords);
            Name = fileName;
        }

        private void parseKeywords(List<string> keywords)
        {
            Keywords = keywords.Select(x => new KeywordEntity(x)).ToList();
        }

    }
}