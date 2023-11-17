using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ReactiveDemo.Component
{
    public class CustomerButton : Button
    {
        static CustomerButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomerButton), new FrameworkPropertyMetadata(typeof(CustomerButton)));
        }

        #region DependencyProperty

        public static readonly DependencyProperty CornerRadiusProperty
            = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomerButton),new PropertyMetadata(null));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        #endregion

        public CustomerButton()
        {
            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();


        }
    }
}
