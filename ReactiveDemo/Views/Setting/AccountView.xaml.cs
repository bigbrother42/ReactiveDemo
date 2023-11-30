using InfrastructureDemo.Util;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ReactiveDemo.Models.UiModel;
using ReactiveDemo.ViewModels.Setting;
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

namespace ReactiveDemo.Views.Setting
{
    /// <summary>
    /// Interaction logic for AccountView.xaml
    /// </summary>
    public partial class AccountView : MetroWindow
    {
        public AccountView()
        {
            InitializeComponent();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AccountViewModel vm
                && AccountDataGrid.SelectedItem is AccountUiModel selectedAccount)
            {
                var result = await MessageUtil.ShowMessageBoxAsync(this, "Warning!", $"Do you want to delete this account? [{selectedAccount.UserName}]");

                if (result == MessageDialogResult.Affirmative)
                {
                    vm.DeleteAccountCommand.Execute(selectedAccount);
                }
            }
        }
    }
}
