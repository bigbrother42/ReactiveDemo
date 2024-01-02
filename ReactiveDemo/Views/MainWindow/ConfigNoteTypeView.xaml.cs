using BaseDemo.Util;
using BaseDemo.Util.Extensions;
using MahApps.Metro.Controls;
using ReactiveDemo.Models.UiModel;
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
    /// Interaction logic for ConfigNoteTypeView.xaml
    /// </summary>
    public partial class ConfigNoteTypeView : MetroWindow
    {
        public ConfigNoteTypeView()
        {
            InitializeComponent();
        }

        private void AccountDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (DataContext is ConfigNoteTypeViewModel vm)
            {
                if (e.Row.DataContext is NoteCategoryTypeUiModel uiModel)
                {
                    if (uiModel.TypeName.IsNullOrEmpty())
                    {
                        var needDeleteItem = vm.NoteTypeCollection.FirstOrDefault(o => o.TypeId == uiModel.TypeId);
                        if (needDeleteItem != null)
                        {
                            vm.NoteTypeCollection.Remove(needDeleteItem);
                        }
                    }
                }
            }
        }

        private void AccountDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (DataContext is ConfigNoteTypeViewModel vm)
            {
                if (e.Row.DataContext is NoteCategoryTypeUiModel uiModel)
                {
                    if (string.IsNullOrEmpty(uiModel.TypeName))
                    {
                        uiModel.TypeId = vm.NoteTypeCollection.Count() == 0 ? 1 : vm.NoteTypeCollection.Max(o => o.TypeId) + 1;
                        uiModel.DisplayOrder = vm.NoteTypeCollection.Count() == 0 ? 1 : vm.NoteTypeCollection.Max(o => o.DisplayOrder) + 1;
                    }
                }
            }
        }
    }
}
