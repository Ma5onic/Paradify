using Castle.Windsor;

namespace web.IoC
{
    public static class ContainerManager
    {
        private static IWindsorContainer _container;

        public static IWindsorContainer Container
        {
            get
            {
                if (_container == null)
                {
                    Create();
                }
                return _container;
            }
        }

        private static void Create()
        {
            _container = new WindsorContainer();
        }
    }
}