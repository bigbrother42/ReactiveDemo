using ReactiveDemo.Subscribers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Filters
{
    public class RegisterEventSubscribersFilter : AbstractFilter
    {
        private IEventSubscriber _login = new ApplicationLogin();

        public override object Handle(object request)
        {
            if (request is LauncherContext context)
            {
                RegisterEventSubscribers(context);
            }
            return base.Handle(request);
        }

        private void RegisterEventSubscribers(LauncherContext context)
        {
            _login.Register(context);
        }
    }
}
