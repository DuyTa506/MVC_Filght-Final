using Autofac;
using Autofac.Integration.Mvc;
using EaseFlight.BLL.Services;
using EaseFlight.DAL.Repositories;
using EaseFlight.DAL.UnitOfWorks;
using System.Linq;

namespace EaseFlight.Web.App_Start
{
    public static class ContainerConfig
    {
        public static IContainer RegisterComponent()
        {
            var builder = new ContainerBuilder();
            // Register mvc controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //// Map interface with class
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            //// Auto mapping all Class in Services namespace(folder) with IClass interface
            builder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
                .Where(t => t.Namespace.Contains("Services") && !t.IsAbstract)
                .AsImplementedInterfaces().InstancePerRequest();

            //// Auto mapping all Class in Repositories namespace(folder) with IClass interface
            builder.RegisterAssemblyTypes(typeof(BaseRepository).Assembly)
                            .Where(t => t.Namespace.Contains("Repositories") && !t.IsAbstract)
                            .AsImplementedInterfaces().InstancePerRequest();

            return builder.Build();
        }
    }
}