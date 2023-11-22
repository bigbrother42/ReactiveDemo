using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Configure
{
    public class ReactiveFrameworkConfigure : IAppConfigure
    {
        public void Configure(LauncherContext context)
        {
            ReactivePropertyScheduler.SetDefault(ImmediateScheduler.Instance);
        }
    }
}
