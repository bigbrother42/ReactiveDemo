using CommonServiceLocator;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.ServiceLocation;

namespace ReactiveDemo.Configure
{
    public class ViewModelLocationProviderConfigure : IAppConfigure
    {
        public void Configure(LauncherContext context)
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory(x => context.Container.Resolve(x));
        }
    }
}
