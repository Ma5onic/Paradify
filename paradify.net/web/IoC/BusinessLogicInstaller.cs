using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using web.Repositories;
using web.Services;
using web.Services.Implementations;

namespace web.IoC
{
    public class BusinessLogicInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ITokenService>().ImplementedBy<TokenService>().LifestylePerWebRequest());
            container.Register(Component.For<IHistoryRepository>().ImplementedBy<HistoryRepository>());
            container.Register(Component.For<IParadifyService>().ImplementedBy<ParadifyService>());
            container.Register(Component.For<IHistoryService>().ImplementedBy<HistoryService>());
            container.Register(Component.For<IUserService>().ImplementedBy<UserService>());
        }
    }
}