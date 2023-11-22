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
using MahApps.Metro.Controls;

namespace ReactiveDemo.Views
{
    /// <summary>
    /// Interaction logic for ReactiveDemoView.xaml
    /// </summary>
    public partial class ReactiveDemoView : MetroWindow
    {
        public ReactiveDemoView()
        {
            InitializeComponent();
        }

        public void TestMethod(string methodParameter)
        {
            Console.WriteLine($"{methodParameter}");
        }
    }
}
