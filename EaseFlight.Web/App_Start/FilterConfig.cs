using EaseFlight.Web.App_Start;
using System.Web.Mvc;

namespace EaseFlight.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // Add sesion check filter for all action
            filters.Add(new SessionCheckActionFilterAttribute());
        }
    }
}