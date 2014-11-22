using Microsoft.Practices.Unity;
using WorkerRole1.Infrastructure.Repository;

namespace WorkerRole1.Infrastructure.IOC
{
    public static class Bootstrapper
    {
        private static IUnityContainer _container;

        public static IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }

        public static IUnityContainer Initialise()
        {
            BuildUnityContainer();

            return _container;
        }

        private static void BuildUnityContainer()
        {
            _container = new UnityContainer();

            RegisterTypes(_container);
            RegisterInstances(_container);
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            // register all your components with the container here
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>), "Repository");
        }

        public static void RegisterInstances(IUnityContainer container)
        {
            if (_container != null)
            {
                // register instances here   
            }
        }
    }
}
