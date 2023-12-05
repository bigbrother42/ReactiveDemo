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
        private string _originalName = string.Empty;

        public NoteView()
        {
            InitializeComponent();

            if (this.DataContext == null)
            {
                this.DataContext = new NoteViewModel();
            }
        }

        private void EditTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is NoteViewModel vm
                && sender is TextBox textBox
                && textBox.DataContext is NoteCategoryUiModel noteCategoryUiModel)
            {
                _originalName = textBox.Text;
            }
        }

        private void EditTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is NoteViewModel vm
                && sender is TextBox textBox
                && textBox.DataContext is NoteCategoryUiModel noteCategoryUiModel)
            {
                if (textBox.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("Please enter category name!", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.Equals(_originalName, textBox.Text) && !string.Equals("New Category", textBox.Text)) return;

                new NoteModel().InsertOrUpdateNoteCategory(noteCategoryUiModel);

                e.Handled = true;
                noteCategoryUiModel.IsEdit = false;
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

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue != null)
            {
                var brusher = new SolidColorBrush(e.NewValue.Value);
                NoteContentRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brusher);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem comboBoxItem)
            {
                NoteContentRichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, double.Parse(comboBoxItem.Content.ToString()));
            }
        }
    }
}
