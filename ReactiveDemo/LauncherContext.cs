using Prism.Events;
using Unity;

namespace ReactiveDemo
{
    public class LauncherContext
    {
        public IUnityContainer Container { get; set; }

        public IEventAggregator EventAggregator { get; set; }

    }
}
