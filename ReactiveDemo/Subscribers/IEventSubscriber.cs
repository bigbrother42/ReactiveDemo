using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Subscribers
{
    public interface IEventSubscriber
    {
        void Register(LauncherContext context);
    }
}
