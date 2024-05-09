using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.Models.Singleton
{
    public class SingletonBase<T> where T : class
    {
        private static readonly Lazy<T> instance = new Lazy<T>(() => CreateInstanceOfT(), isThreadSafe: true);

        public static T Instance => instance.Value;

        protected SingletonBase()
        {

        }

        private static T CreateInstanceOfT()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }
    }
}
