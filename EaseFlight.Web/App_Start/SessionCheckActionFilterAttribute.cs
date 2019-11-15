using EaseFlight.Web.WebUtilities;
using System.Web.Mvc;
using System.Web.Routing;

namespace EaseFlight.Web.App_Start
{
    public class SessionCheckActionFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (NeedToCheckSession(filterContext))
            {
                if (SessionUtility.IsSessionAlive() == false)
                {
                    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    string actionName = filterContext.ActionDescriptor.ActionName;
                    var callBack = controllerName + "/" + actionName;

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        { "action", "Login" },
                        { "controller", "Account" },
                        { "Area", "Admin" },
                        { "callUrl", callBack}
                    });
                }
            }
        }
        private bool NeedToCheckSession(ActionExecutingContext filterContext)
        {
            var currentArea = filterContext.RouteData.DataTokens["area"] ?? string.Empty;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;

            if (currentArea.ToString().ToLower().Equals("admin") && !(controllerName.Equals("Account") && actionName.Equals("Login")))
                return true;

            return false;
        }
    }
}