﻿using ControlzEx.Theming;
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

        private void AccentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTheme = e.AddedItems.OfType<Theme>().FirstOrDefault();
            if (selectedTheme != null)
            {
                ThemeManager.Current.ChangeTheme(Application.Current, selectedTheme);
            }
        }
    }
}
