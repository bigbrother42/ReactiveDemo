using MahApps.Metro.Controls;
using ReactiveDemo.ViewModels.Login;
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

namespace ReactiveDemo.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : MetroWindow
    {
        public LoginView()
        {
            InitializeComponent();

            UserNameTextBox.Focus();
        }

        private void PasswordTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DataContext is LoginViewModel vm)
                {
                    vm.LoginCommand.Execute();
                }
            }
        }
    }
}
