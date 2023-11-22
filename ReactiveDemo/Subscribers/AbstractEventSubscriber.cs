using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ReactiveDemo.Subscribers
{
    public class AbstractEventSubscriber : IEventSubscriber
    {
        protected LauncherContext Context;

        protected IUnityContainer Container
        {
            get
            {
                return Context?.Container;
            }
        }

        protected IEventAggregator EventAggregator
        {
            get
            {
                return Context?.EventAggregator;
            }
        }

        public virtual void Register(LauncherContext context)
        {
            Context = context;
        }

        public void UnSubscribeEvent(EventBase eventBase, SubscriptionToken token)
        {
            if (token != null)
            {
                eventBase?.Unsubscribe(token);
            }
        }
    }
}
