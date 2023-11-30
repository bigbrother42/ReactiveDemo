using ReactiveDemo.Component;
using InfrastructureDemo.Constants.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReactiveDemo.Schema
{
    public class BaseSchema : ISchema
    {
        protected Enum ColorEnum { get; set; }
        protected Enum ImageSourceEnum { get; set; }

        private ResourceDictionary ThemeDictionary
        {
            get
            {
                return Application.Current.Resources.MergedDictionaries.FirstOrDefault(r =>
                    !(r is SharedResourceDictionary));
            }
        }

        public virtual void ChangeSystemTheme(string theme)
        {
            
        }

        protected void ReplaceThemeResource()
        {
            if (ColorEnum != null)
            {
                var uri = new Uri(ColorEnum.GetResource());
                var imageUri = new Uri(ImageSourceEnum.GetResource());
                ThemeDictionary?.MergedDictionaries.Clear();
                ThemeDictionary?.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
                ThemeDictionary?.MergedDictionaries.Add(new ResourceDictionary() { Source = imageUri });
            }
        }
    }
}
