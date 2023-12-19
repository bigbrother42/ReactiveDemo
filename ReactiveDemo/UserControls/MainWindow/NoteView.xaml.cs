using BaseDemo.Util;
using InfrastructureDemo.Util;
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
using System.Windows.Markup;
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
        private bool isInit;

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

        private async void EditTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is NoteViewModel vm
                && sender is TextBox textBox
                && textBox.DataContext is NoteCategoryUiModel noteCategoryUiModel)
            {
                if (textBox.Text.IsNullOrEmpty())
                {
                    //MessageBox.Show("Please enter category name!", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                    vm.SelectedNoteType.Value.CategoryList.Remove(noteCategoryUiModel);

                    return;
                }

                if (string.Equals(_originalName, textBox.Text) && !string.Equals("New Category", textBox.Text)) return;

                if (!isInit) return;

                await new NoteModel().InsertOrUpdateNoteCategory(noteCategoryUiModel);

                e.Handled = true;
                noteCategoryUiModel.IsEdit = false;
            }
        }

        public void AddNoteCategory()
        {
            if (NoteCategoryListView.SelectedItem is NoteCategoryUiModel noteCategoryUiModel)
            {
                noteCategoryUiModel.IsEdit = true;
            }
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

        private void NoteContentRichTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Keyboard.IsKeyUp(Key.S))
                {
                    if (DataContext is NoteViewModel vm)
                    {
                        vm.SaveContentCommand.Execute();
                    }
                }
            }
        }

        private void EditTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                isInit = true;
                _originalName = textBox.Text;
            }
        }

        private void NoteCategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NoteContentRichTextBox.Dispatcher.BeginInvoke(new Action(() => {
                NoteContentRichTextBox.Focus();
            }), System.Windows.Threading.DispatcherPriority.Background, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ColorPiker.SelectedColor != null)
            {
                var brusher = new SolidColorBrush((Color)ColorPiker.SelectedColor);
                NoteContentRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brusher);
            }
        }

        private void FontSizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (FontSizeComboBox.SelectedItem is ComboBoxItem comboBoxItem)
            {
                NoteContentRichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, double.Parse(comboBoxItem.Content.ToString()));
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BoldCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            NoteContentRichTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }

        private void BoldCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            NoteContentRichTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
        }

        private void ItelicCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            NoteContentRichTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
        }

        private void ItelicCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            NoteContentRichTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
        }
    }
}
