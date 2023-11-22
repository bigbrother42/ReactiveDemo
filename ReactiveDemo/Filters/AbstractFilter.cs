using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Filters
{
    public class AbstractFilter : IFilter
    {
        private IFilter _nextFilter;

        public IFilter SetNext(IFilter filter)
        {
            _nextFilter = filter;

            return filter;
        }

        public virtual object Handle(object request)
        {
            if (_nextFilter != null)
            {
                return _nextFilter.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }
}
