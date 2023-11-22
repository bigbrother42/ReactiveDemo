using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Events
{
    public class LoginEvents
    {
        public class UserLoginRequestEvent : PubSubEvent<int> { }
    }
}
