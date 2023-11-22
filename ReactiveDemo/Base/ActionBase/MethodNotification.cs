using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Base.ActionBase
{
    public class MethodNotification : Notification
    {
        public object[] Parameters { get; set; }

        public object ReturnParameter { get; set; }

        public MethodNotification()
        {

        }

        public MethodNotification(object[] parameters)
        {
            this.Parameters = parameters;
        }
    }
}
