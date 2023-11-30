using InfrastructureDemo.Constants.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Schema
{
    class NormalSchema : BaseSchema
    {
        public override void ChangeSystemTheme(string theme)
        {
            base.ChangeSystemTheme(theme);

            ColorEnum = EnumExtension.ConvertCodeToEnum<ConfigEnum.SystemTheme>(theme);
            ImageSourceEnum = EnumExtension.ConvertCodeToEnum<ConfigEnum.SystemImageTheme>(theme);

            ReplaceThemeResource();
        }
    }
}
