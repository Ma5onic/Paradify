using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using web.App_Start;
using web.IoC;
using web.Filters;

namespace web
{
    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer _container;

        protected void Application_Start()
        {
            ConfigureWindsor(GlobalConfiguration.Configuration);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(configuration => WebApiConfig.Register(configuration, _container));
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            IocContainer.Setup();
            log4net.Config.XmlConfigurator.Configure();
            GlobalFilters.Filters.Add(new CustomHandleError());
        }

        public static void ConfigureWindsor(HttpConfiguration configuration)
        {
            _container = ContainerManager.Container;

            _container.Install(FromAssembly.This());

            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));

            var dependencyResolver = new WindsorDependencyResolver(_container);

            configuration.DependencyResolver = dependencyResolver;
        }

        protected void Application_End()
        {
            _container.Dispose();
            base.Dispose();
        }
    }
}
