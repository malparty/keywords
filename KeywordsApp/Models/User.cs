
using Microsoft.AspNetCore.Identity;

namespace KeywordsApp.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
    }

}