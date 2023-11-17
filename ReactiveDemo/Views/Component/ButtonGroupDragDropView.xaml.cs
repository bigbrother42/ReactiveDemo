using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace ReactiveDemo.Views.Component
{
    /// <summary>
    /// Interaction logic for ButtonGroupDragDropView.xaml
    /// </summary>
    public partial class ButtonGroupDragDropView : MetroWindow
    {
        public ButtonGroupDragDropView()
        {
            InitializeComponent();
        }

        private void UIElement_OnDragEnter(object sender, DragEventArgs e)
        {
            
        }

        private void UIElement_OnDragLeave(object sender, DragEventArgs e)
        {
            
        }

        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button dragButton)
            {
                DataObject data = new DataObject("ButtonData", dragButton.Content);

                /*// 创建按钮的拖拽副本
                Image dragImage = CreateDragImage(dragButton);

                // 获取鼠标指针在按钮上的位置
                Point mousePos = e.GetPosition(dragButton);

                // 设置拖拽副本的位置
                Canvas.SetLeft(dragImage, mousePos.X - dragImage.ActualWidth / 2);
                Canvas.SetTop(dragImage, mousePos.Y - dragImage.ActualHeight / 2);

                // 添加拖拽副本到窗口中
                MainGrid.Children.Add(dragImage);*/

                DragDrop.DoDragDrop(dragButton, data, DragDropEffects.Copy | DragDropEffects.Move);

                /*// 移除拖拽副本
                MainGrid.Children.Remove(dragImage);*/
            }
        }

        private Image CreateDragImage(Button button)
        {
            // 创建按钮的拖拽副本
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)button.ActualWidth, (int)button.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(button);

            Image dragImage = new Image();
            dragImage.Source = rtb;
            dragImage.Width = button.ActualWidth;
            dragImage.Height = button.ActualHeight;

            // 设置拖拽副本的透明度
            dragImage.Opacity = 0.7;

            return dragImage;
        }
    }
}
