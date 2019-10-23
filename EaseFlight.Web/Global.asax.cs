using Autofac.Integration.Mvc;
using EaseFlight.Models.ModelBinders;
using EaseFlight.Web.App_Start;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EaseFlight.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.DefaultBinder = new CustomModelBinder();
            var container = ContainerConfig.RegisterComponent();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.IsSecureConnection.Equals(false) && HttpContext.Current.Request.IsLocal.Equals(false))
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl);
            }
        }
    }
}