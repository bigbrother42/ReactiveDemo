using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.ServiceLocation;

namespace ReactiveDemo.Configure
{
    public class ServiceLocatorConfigure : IAppConfigure
    {
        public void Configure(LauncherContext context)
        {
            var provider = new UnityServiceLocator(context.Container);
            ServiceLocator.SetLocatorProvider(() => provider);
        }
    }
}
