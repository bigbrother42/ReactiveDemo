using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace ReactiveDemo.Views.MainWindow
{
    /// <summary>
    /// Interaction logic for SearchNoteView.xaml
    /// </summary>
    public partial class SearchNoteView : MetroWindow
    {
        public SearchNoteView()
        {
            InitializeComponent();
        }

        private void DataGridRow_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is SearchNoteViewModel vm)
            {
                vm.ExecuteCommand.Execute();
            }
        }
    }
}
