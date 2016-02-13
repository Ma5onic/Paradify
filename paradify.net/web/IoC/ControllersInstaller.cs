using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace web.IoC
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly()
                .Pick().If(t => t.Name.EndsWith("Controller"))
                .Configure(configure => configure.Named(configure.Implementation.Name))
                .LifestylePerWebRequest());

            container.Register(Classes.FromThisAssembly()
            .BasedOn<ApiController>()
            .LifestylePerWebRequest());
        }
    }
}