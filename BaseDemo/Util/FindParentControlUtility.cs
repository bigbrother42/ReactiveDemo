using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace BaseDemo.Util
{
    public static class FindParentControlUtility
    {
        internal static DependencyObject FindVisualTreeRoot(this DependencyObject d)
        {
            var current = d;
            var result = d;

            while (current != null)
            {
                result = current;
                if (current is Visual || current is Visual3D) break;

                // If we're in Logical Land then we must walk up the logical tree
                // until we find a Visual/Visual3D to get us back to Visual Land.
                current = LogicalTreeHelper.GetParent(current);
            }

            return result;
        }

        public static T FindVisualParentByName<T>(this DependencyObject obj, string name) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(obj.FindVisualTreeRoot());
            while (parent != null)
            {
                var element = parent as T;
                if (element != null && element.Name == name)
                    return element;

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        public static T FindVisualParent<T>(DependencyObject obj, string name = null) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T element && (element.Name == name || string.IsNullOrEmpty(name)))
                {
                    return element;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }
    }
}
