
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KeywordsApp.Models
{
    [Table("Users")]
    [Index(nameof(FirstName), nameof(LastName))]
    public class User : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "character varying(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "character varying(100)")]
        public string LastName { get; set; }
    }

}