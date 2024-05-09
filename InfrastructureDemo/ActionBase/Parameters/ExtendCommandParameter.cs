using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InfrastructureDemo.ActionBase.Parameters
{
    public class ExtendCommandParameter
    {
        public DependencyObject Sender { get; set; }

        public EventArgs EventArgs { get; set; }

        public object Parameter { get; set; }
    }
}
