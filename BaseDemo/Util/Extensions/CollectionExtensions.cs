﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.Util.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Get all DbSet properties of DbContext class
        /// </summary>
        /// <param name="type">The type of the DbContext</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetAllSetProperties(this Type type)
        {
            return type.GetProperties().AsEnumerable().Where(o => o.PropertyType.IsGenericType);
        }

        public static int ExRemoveAll<T>(this ObservableCollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }
    }
}
