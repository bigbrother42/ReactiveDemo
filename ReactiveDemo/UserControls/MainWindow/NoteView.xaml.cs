using BaseDemo.Util;
using InfrastructureDemo.Util;
using Prism.Services.Dialogs;
using ReactiveDemo.Models.MainWindow;
using ReactiveDemo.Models.UiModel;
using ReactiveDemo.ViewModels.MainWindow;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static InfrastructureDemo.Constans.Enum.EnumConstants;

namespace ReactiveDemo.UserControls.MainWindow
{
    /// <summary>
    /// Interaction logic for NoteView.xaml
    /// </summary>
    public partial class NoteView : UserControl
    {
        public NoteView()
        {
            InitializeComponent();

            if (this.DataContext == null)
            {
                this.DataContext = new NoteViewModel();
            }
        }

        private async void EditTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is NoteViewModel vm
                && sender is TextBox textBox
                && textBox.DataContext is NoteCategoryUiModel noteCategoryUiModel)
            {
                if (textBox.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("Please enter category name!", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    noteCategoryUiModel.IsEdit = false;
                }

                e.Handled = true;

                var noteModel = new NoteModel();
                await noteModel.InsertNoteCategory(noteCategoryUiModel);
            }
        }

        public void AddNoteCategory()
        {
            
        }

        private void EditTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Visibility == Visibility.Visible)
                {
                    textBox.Focus();
                    textBox.SelectAll();
                }
                else if (textBox.Visibility == Visibility.Collapsed)
                {
                    textBox.RaiseEvent(new RoutedEventArgs(LostFocusEvent));
                }
            }
        }

        private void EditTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (e.Key == Key.Enter)
                {
                    textBox.RaiseEvent(new RoutedEventArgs(LostFocusEvent));
                }
            }
        }
    }
}
