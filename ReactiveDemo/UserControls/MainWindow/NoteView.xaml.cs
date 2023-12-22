using BaseDemo.Helpers.Component;
using BaseDemo.Util;
using InfrastructureDemo.Util;
using ReactiveDemo.Models.MainWindow;
using ReactiveDemo.Models.UiModel;
using ReactiveDemo.ViewModels.MainWindow;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

            DataObject.AddPastingHandler(this, OnPaste);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var richTextBox = GetSelectedTabItemRichTextBox();
            if (richTextBox != null)
            {
                richTextBox.Focus();

                var dataFormats = e.DataObject.GetFormats();
                object data;

                richTextBox.BeginChange();

                for (int i = dataFormats.Length - 1; i >= 0; i--)
                {
                    string format = dataFormats[i];
                    switch (format)
                    {
                        case "Xaml":
                            data = e.DataObject.GetData(DataFormats.Xaml);
                            if (data != null)
                            {
                                // TODO
                            }

                            break;
                        case "DeviceIndependentBitmap":
                        case "System.Windows.Media.Imaging.BitmapSource":
                        case "System.Drawing.Bitmap":
                        case "Bitmap":
                            if (e.DataObject.GetDataPresent(DataFormats.Bitmap))
                            {
                                BitmapSource image = Clipboard.GetImage();

                                if (!Directory.Exists(GlobalData.IMAGE_PATH))
                                {
                                    Directory.CreateDirectory(GlobalData.IMAGE_PATH);
                                }

                                var filePath = $@"{GlobalData.IMAGE_PATH}\{Guid.NewGuid()}";
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    BitmapEncoder encoder = new PngBitmapEncoder();
                                    encoder.Frames.Add(BitmapFrame.Create(image));
                                    encoder.Save(fileStream);
                                }

                                InlineUIContainer uIContainer = new InlineUIContainer();
                                uIContainer.Child = new Image
                                {
                                    Source = new BitmapImage(new Uri(filePath)),
                                    Width = image.Width,
                                    Height = image.Height
                                };

                                Paragraph imageParagraph = new Paragraph();
                                imageParagraph.Inlines.Add(uIContainer);
                                richTextBox.Document.Blocks.Add(imageParagraph);
                                var ss = XamlWriter.Save(richTextBox.Document);

                                e.CancelCommand();
                            }

                            break;
                        case "System.String":
                            data = e.DataObject.GetData(DataFormats.StringFormat);
                            richTextBox.Selection.Text = data?.ToString();
                            e.CancelCommand();

                            break;
                        case "Text":
                            data = e.DataObject.GetData(DataFormats.StringFormat);
                            richTextBox.Selection.Text = data?.ToString();
                            e.CancelCommand();

                            break;
                    }
                }

                richTextBox.EndChange();

                e.Handled = true;
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

                if (string.Equals(_originalName, textBox.Text) && !string.Equals(NoteModel.NEW_CATEGORY_NAME, textBox.Text))
                {
                    noteCategoryUiModel.IsEdit = false;
                    return;
                };

                if (!isInit) return;

                await new NoteModel().InsertOrUpdateNoteCategory(noteCategoryUiModel);

                e.Handled = true;
                noteCategoryUiModel.IsEdit = false;
            }
        }

        public void AddNoteCategory()
        {
            if (NoteCategoryTabControl.SelectedItem is NoteCategoryUiModel noteCategoryUiModel)
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

        private RichTextBox GetSelectedTabItemRichTextBox()
        {
            var selectedTabItemContentPresenter = NoteCategoryTabControl.Template.FindName("PART_SelectedContentHost", NoteCategoryTabControl) as ContentPresenter;
            if (selectedTabItemContentPresenter != null)
            {
                return selectedTabItemContentPresenter.FindVisualChild<RichTextBox>();
            }

            return null;
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue != null)
            {
                var brusher = new SolidColorBrush(e.NewValue.Value);
                var richTextBox = GetSelectedTabItemRichTextBox();
                richTextBox?.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brusher);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem comboBoxItem)
            {
                var richTextBox = GetSelectedTabItemRichTextBox();
                richTextBox?.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, double.Parse(comboBoxItem.Content.ToString()));
            }
        }

        private void NoteContentRichTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Keyboard.IsKeyUp(Key.S))
                {
                    if (DataContext is NoteViewModel vm && sender is RichTextBox richTextBox)
                    {
                        RichTextBoxHelper.SetDocumentXaml(richTextBox, XamlWriter.Save(richTextBox.Document));

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

                if (textBox.Visibility == Visibility.Visible)
                {
                    textBox.Focus();
                    textBox.SelectAll();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ColorPiker.SelectedColor != null)
            {
                var brusher = new SolidColorBrush((Color)ColorPiker.SelectedColor);
                var richTextBox = GetSelectedTabItemRichTextBox();
                richTextBox?.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brusher);
            }
        }

        private void FontSizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (FontSizeComboBox.SelectedItem is ComboBoxItem comboBoxItem)
            {
                var richTextBox = GetSelectedTabItemRichTextBox();
                richTextBox?.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, double.Parse(comboBoxItem.Content.ToString()));
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BoldCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var richTextBox = GetSelectedTabItemRichTextBox();
            richTextBox?.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }

        private void BoldCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var richTextBox = GetSelectedTabItemRichTextBox();
            richTextBox?.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
        }

        private void ItelicCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var richTextBox = GetSelectedTabItemRichTextBox();
            richTextBox?.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
        }

        private void ItelicCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var richTextBox = GetSelectedTabItemRichTextBox();
            richTextBox?.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
        }

        private void NoteContentRichTextBox_PreviewDrop(object sender, DragEventArgs e)
        {
            if (DataContext is NoteViewModel vm)
            {
                var data = e.Data as DataObject;
                var fileDropList = data?.GetFileDropList();

                if (fileDropList == null || fileDropList.Count != 1)
                {
                    return;
                }

                var fileName = fileDropList[0];

                e.Handled = true;

            }
        }

        private void NoteContentRichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is RichTextBox richTextBox)
            {
                RichTextBoxHelper.SetDocumentXaml(richTextBox, XamlWriter.Save(richTextBox.Document));

                //RichTextBoxColorPickerPopUp.IsOpen = false;
            }
        }

        private void NoteContentRichTextBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is RichTextBox richTextBox)
            {
                if (richTextBox.Selection.Text.IsNullOrEmpty()) {
                    //RichTextBoxColorPickerPopUp.IsOpen = false;

                    return;
                }

                RichTextBoxColorPickerPopup.IsOpen = true;
            }
        }

        private void NoteContentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is RichTextBox richTextBox)
            {
                if (richTextBox.Selection.Text.IsNullOrEmpty())
                {
                    RichTextBoxColorPickerPopup.IsOpen = false;
                }
            }
        }

        private void RichTextBoxColorPickerPopup_Opened(object sender, EventArgs e)
        {
            if (sender is Popup popup)
            {
                PopupBoldCheckbox.IsChecked = false;
                PopupItalicCheckbox.IsChecked = false;
                PopupColorPalette.SelectedItem = null;
            }
        }

        private void ColorPalette_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PopupColorPalette.SelectedItem is Color selectedItem)
            {
                //var brusher = new SolidColorBrush((Color)ColorConverter.ConvertFromString(selectedColor));
                var brusher = new SolidColorBrush(selectedItem);
                var richTextBox = GetSelectedTabItemRichTextBox();
                richTextBox?.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brusher);
            }
        }
    }
}
