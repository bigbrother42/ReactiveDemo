using ControlzEx.Theming;
using InfrastructureDemo.Util;
using ReactiveDemo.Constants;
using ReactiveDemo.ViewModels.MainWindow;
using SharedDemo.GlobalData;
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
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

            if (this.DataContext == null)
            {
                this.DataContext = new SettingsViewModel();
            }

            ThemeComboBox.SelectedValue = ThemeManager.Current.GetTheme(GlobalData.SystemTheme); ;
        }

        private void ImportTile_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    
                }
            }
        }

        private async void Border_Drop(object sender, DragEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
            {
                var data = e.Data as DataObject;
                var text = data?.GetText();
                var fileDropList = data?.GetFileDropList();

                if (fileDropList == null || fileDropList.Count == 0)
                {
                    return;
                }

                var res = MessageBox.Show(MessageBoxConstant.IMPORT_FILE_CONFIRM_MESSAGE, MessageBoxConstant.TITLE_WARNING, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    if (fileDropList.Count > 1)
                    {
                        MessageBox.Show(MessageBoxConstant.IMPORT_FILE_LIMIT_ONE_FILE_MESSAGE, MessageBoxConstant.TITLE_WARNING, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var fileName = fileDropList[0];
                    await vm.ImportForDrop(fileName);
                }
            }
        }
    }
}
