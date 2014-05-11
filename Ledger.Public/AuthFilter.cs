using System.Web.Mvc;

namespace Ledger.Public
{
    public class AuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var token = filterContext.HttpContext.Request.QueryString["token"];
            if (string.IsNullOrEmpty(token))
                filterContext.Result = new HttpUnauthorizedResult("Access denied: token not found.");
            else if(!Token.IsAuthorized(token))
                filterContext.Result = new HttpUnauthorizedResult("Access denied: invalid token.");
        }
    }
}