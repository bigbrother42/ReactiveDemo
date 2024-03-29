﻿using ReactiveDemo.ViewModels.MainWindow;
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

namespace ReactiveDemo.UserControls.MainWindow
{
    /// <summary>
    /// Interaction logic for PrivateView.xaml
    /// </summary>
    public partial class PrivateView : UserControl
    {
        public PrivateView()
        {
            InitializeComponent();

            if (this.DataContext == null)
            {
                this.DataContext = new PrivateViewModel();
            }
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is PrivateViewModel privateViewModel)
            {
                privateViewModel.AccountViewCommand.Execute();
            }
        }
    }
}
