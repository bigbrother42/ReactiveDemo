using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ReactiveDemo.Models.UiModel
{
    public class NoteCategoryUiModel : BindableBase
    {
        private int _typeId;
        public int TypeId
        {
            get => _typeId;
            set => SetProperty(ref _typeId, value);
        }

        private int _categorySeq;
        public int CategorySeq
        {
            get => _categorySeq;
            set => SetProperty(ref _categorySeq, value);
        }

        private string _categoryName = string.Empty;
        public string CategoryName
        {
            get => _categoryName;
            set => SetProperty(ref _categoryName, value);
        }

        private bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }
    }
}
