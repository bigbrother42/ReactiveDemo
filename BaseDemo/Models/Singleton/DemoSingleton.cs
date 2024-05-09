using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.Models.Singleton
{
    public class DemoSingleton : SingletonBase<DemoSingleton>
    {
        private DemoSingleton()
        {

        }

        private bool _isFirstLoad = true;
        private readonly object _lock = new object();

        public bool IsFirstLoad
        {
            get
            {
                lock (_lock)
                {
                    return _isFirstLoad;
                }
            }
            set
            {
                lock (_lock)
                {
                    _isFirstLoad = value;
                }
            }
        }
    }
}
