
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using KeywordsApp.Models.File;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KeywordsApp.Models
{
    [Table("Users")]
    [Index(nameof(FirstName), nameof(LastName))]
    public class UserEntity : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "character varying(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "character varying(100)")]
        public string LastName { get; set; }

        public IEnumerable<FileEntity> CsvFiles { get; set; }
    }

}