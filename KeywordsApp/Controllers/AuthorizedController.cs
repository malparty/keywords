using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KeywordsApp.Controllers
{

    [Authorize]
    public class AuthorizedController : Controller
    {
        public string UserId { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Send 403 in case userId is not valid
            if (string.IsNullOrEmpty(UserId))
            {
                context.Result = new Http403Result();
            }
        }

    }

    internal class Http403Result : ActionResult
    {
        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = 403;
        }
    }
}