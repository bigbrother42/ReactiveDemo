using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.Util.Extensions
{
    public static class EnumerableExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> self) => self == null || !self.Any();

        public static bool AnyAndNotNull<T>(this IEnumerable<T> self) => !self.IsNullOrEmpty();

        public static List<T> ToListOrEmpty<T>(this IEnumerable<T> self) =>
            self?.ToList() ?? Enumerable.Empty<T>().ToList();

        public static T[] ToArrayOrEmpty<T>(this IEnumerable<T> self) => self?.ToArray() ?? Array.Empty<T>();
    }
}
