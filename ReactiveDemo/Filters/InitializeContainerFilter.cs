using Prism.Events;
using ReactiveDemo.Configure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ReactiveDemo.Filters
{
    public class InitializeContainerFilter : AbstractFilter
    {
        private IAppConfigure _autoMapperConfigure = new AutoMapperConfigure();

        private IAppConfigure _vmLocationProviderConfigure = new ViewModelLocationProviderConfigure();

        private IAppConfigure _reactiveFrameworkConfigure = new ReactiveFrameworkConfigure();

        private IAppConfigure _serviceLocatorConfigure = new ServiceLocatorConfigure();

        public override object Handle(object request)
        {
            if (request is LauncherContext context)
            {
                InitializeContainer(context);
            }

            return base.Handle(request);
        }

        private void InitializeContainer(LauncherContext context)
        {
            context.Container.RegisterInstance(typeof(IUnityContainer), context.Container);
            context.Container.RegisterInstance(typeof(IEventAggregator), context.EventAggregator);

            _autoMapperConfigure.Configure(context);
            _vmLocationProviderConfigure.Configure(context);
            _reactiveFrameworkConfigure.Configure(context);
            _serviceLocatorConfigure.Configure(context);
        }
    }
}
