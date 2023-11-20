using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Schema
{
    public class ThemeSchemaProvider
    {
        internal static ISchema GetThemeSchema()
        {
            var mode = "Normal";

            switch (mode)
            {
                case "Normal":
                    return new NormalSchema();
                default: return new NormalSchema();
            }
        }
    }
}
