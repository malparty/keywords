using System.Security.Claims;
using System.Threading.Tasks;
using KeywordsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace KeywordsApp.Areas.Identity
{
    public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
    {
        public UserClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        // Add Names properties to the Razore User object
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("LastName", user.LastName ?? ""));
            identity.AddClaim(new Claim("FirstName", user.FirstName ?? ""));
            return identity;
        }
    }
}