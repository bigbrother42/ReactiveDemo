using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InfrastructureDemo.Constants.Enum.EnumExtension;

namespace InfrastructureDemo.Constants.Enum
{
    public class ConfigEnum
    {
        public enum SystemTheme
        {
            [Description("Normal"), Code("Normal"),
             Resource("pack://application:,,,/ReactiveDemo;component/Themes/Theme.xaml")]
            Normal = 0,
        }

        public enum SystemImageTheme
        {
            [Description("Normal"), Code("Normal"),
             Resource("pack://application:,,,/ReactiveDemo;component/Themes/ImageTheme.xaml")]
            Normal = 0,
        }
    }
}
