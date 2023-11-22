using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Filters
{
    public class BaseFilter : AbstractFilter
    {
        public override object Handle(object request)
        {
            return base.Handle(request);
        }
    }
}
