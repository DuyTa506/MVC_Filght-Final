using System.Collections.Generic;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            var controllerAdmin = new List<string> { "Account", "Airport", "Country", "Flight", "Home", "Plane", "Ticket" };

            //Set default route for all controller
            foreach(var ctroller in controllerAdmin)
            {
                var name = "Admin_"+ ctroller +"_Default";
                var url = "Admin/"+ ctroller +"/{action}/{id}";

                context.MapRoute(name, url , new
                {
                    controller = ctroller,
                    action = "Index",
                    id = UrlParameter.Optional,
                });
            }

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}