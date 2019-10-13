using EaseFlight.Models.ModelBinders;
using EaseFlight.Web.App_Start;
using Autofac.Integration.Mvc;
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
    }
}