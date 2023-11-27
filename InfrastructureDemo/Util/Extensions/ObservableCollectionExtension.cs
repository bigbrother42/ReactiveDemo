using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Util.Extensions
{
    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<T> SetOrUpdate<T>(this ObservableCollection<T> self, IEnumerable<T> values)
        {
            if (self == null)
            {
                self = new ObservableCollection<T>();
            }
            else
            {
                self.Clear();
            }

            if (values.AnyAndNotNull())
            {
                self.AddRange(values);
            }

            return self;
        }
    }
}
