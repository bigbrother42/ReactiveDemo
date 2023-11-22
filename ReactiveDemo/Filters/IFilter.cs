using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Filters
{
    public interface IFilter
    {
        IFilter SetNext(IFilter filter);

        object Handle(object request);
    }
}
