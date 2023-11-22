using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace ReactiveDemo.Behavior.ButtonBehavior
{
    public class TestButtonBehavior : Behavior<Button>
    {
        private Brush _originalBackgroundColor;

        protected override void OnAttached()
        {
            base.OnAttached();

            _originalBackgroundColor = AssociatedObject.Background;

            AssociatedObject.MouseEnter += AssociatedObject_MouseEnter;
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        private void AssociatedObject_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AssociatedObject.Background = Brushes.Orange;
        }

        private void AssociatedObject_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AssociatedObject.Background = _originalBackgroundColor;
        }
    }
}
