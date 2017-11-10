using MultiPull.EventStore.Services;
using MultiPull.EventStore.StoreTypes;
using Unity;
using Unity.Lifetime;

namespace MultiPull.EventStore.Unity
{
    public class DependencyContainer
    {
        public static IUnityContainer Register()
        {
            var container = new UnityContainer();

            container.RegisterType(typeof(EventStoreContext), new HierarchicalLifetimeManager());

            container.RegisterType(typeof(DataContext<>));

            container.RegisterType(typeof(OrderService));

            return container;
        }
    }
}