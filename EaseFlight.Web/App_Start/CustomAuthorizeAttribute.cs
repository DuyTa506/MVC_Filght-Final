using EaseFlight.Web.WebUtilities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EaseFlight.Web.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = true;

            if (SessionUtility.IsSessionAlive()) //Only check authorize with user logged
                if (string.IsNullOrEmpty(this.Roles) == false)
                {
                    var roles = this.Roles.Split(' ');
                    var currentUserRoles = SessionUtility.GetLoggedUser().AccountType.Roles;
                    var rolesList = currentUserRoles.Split(' ');

                    authorized = roles.Intersect(rolesList).Count() > 0;
                }

            return authorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //Redirect to error page if user Unauthorized
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "action", "AccessDenied" },
                { "controller", "Account" },
                { "Area", string.Empty }
            });
        }
    }
}