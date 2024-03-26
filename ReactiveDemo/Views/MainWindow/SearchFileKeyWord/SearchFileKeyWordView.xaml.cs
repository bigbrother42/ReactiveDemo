using InfrastructureDemo.Util;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ReactiveDemo.ViewModels.MainWindow.SearchFileKeyWord;
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

namespace ReactiveDemo.Views.MainWindow.SearchFileKeyWord
{
    /// <summary>
    /// Interaction logic for SearchFileKeyWordView.xaml
    /// </summary>
    public partial class SearchFileKeyWordView : MetroWindow
    {
        public SearchFileKeyWordView()
        {
            InitializeComponent();
        }

        private async void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var result = await MessageUtil.ShowMessageBoxAsync(this, "Warning!", $"Do you want to cancel this search?", MessageBoxType.Normal);

            if (result == MessageDialogResult.Affirmative)
            {
                if (DataContext is SearchFileKeyWordViewModel vm)
                {
                    vm.CancelCommand.Execute();
                }
            }
        }
    }
}
