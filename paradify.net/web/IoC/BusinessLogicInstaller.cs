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
            container.Register(Component.For<ITokenCookieService>().ImplementedBy<TokenCookieService>().LifestylePerWebRequest());
            container.Register(Component.For<IUserRepository>().ImplementedBy<UserRepository>());
            container.Register(Component.For<IParadifyService>().ImplementedBy<ParadifyService>());
            container.Register(Component.For<IUserService>().ImplementedBy<UserService>());
            container.Register(Component.For<ISessionService>().ImplementedBy<SessionService>());
            container.Register(Component.For<IPlaylistService>().ImplementedBy<PlaylistService>());
        }
    }
}