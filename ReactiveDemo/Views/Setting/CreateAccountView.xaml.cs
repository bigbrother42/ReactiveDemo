using InfrastructureDemo.Util;
using MahApps.Metro.Controls;
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
using static InfrastructureDemo.Constans.Enum.EnumConstants;

namespace ReactiveDemo.Views.Setting
{
    /// <summary>
    /// Interaction logic for CreateAccountView.xaml
    /// </summary>
    public partial class CreateAccountView : MetroWindow
    {
        public CreateAccountView()
        {
            InitializeComponent();

            UserNameTextBox.Focus();
        }

        public async void UserExist()
        {
            var result = await MessageUtil.ShowMessageBoxAsync(this, "Warning!", $"[{UserNameTextBox.Text}] is exist! \r\nPlease enter another username.", MessageBoxType.OK);
        }
    }
}
