using System.Web.Mvc;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace web.IoC
{
    public static class IocContainer
    {
        private static IWindsorContainer _windsorContainer;
        public static void Setup()
        {
            _windsorContainer = new WindsorContainer().Install(FromAssembly.This());

            WindsorControllerFactory controllerFactory = new WindsorControllerFactory(_windsorContainer.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}