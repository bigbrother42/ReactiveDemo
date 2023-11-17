using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReactiveDemo.Component
{
    public class SharedResourceDictionary : ResourceDictionary
    {
        public static Dictionary<Uri, ResourceDictionary> SharedDictinaries = new Dictionary<Uri, ResourceDictionary>();

        private Uri _sourceUri;

        private static bool IsInDesignMode =>
            (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty,
                typeof(DependencyObject)).Metadata.DefaultValue;

        public new Uri Source
        {
            get => IsInDesignMode ? base.Source : _sourceUri;
            set
            {
                _sourceUri = value;


                if (!SharedDictinaries.ContainsKey(value))
                {
                    try
                    {

                        base.Source = value;
                    }
                    catch (Exception exp)
                    {

                        if (!IsInDesignMode)
                            throw;
                    }

                    SharedDictinaries.Add(value, this);
                }
                else
                {

                    MergedDictionaries.Add(SharedDictinaries[value]);
                }
            }
        }
    }
}
