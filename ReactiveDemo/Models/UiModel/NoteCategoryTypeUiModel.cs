using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Models.UiModel
{
    public class NoteCategoryTypeUiModel : BindableBase
    {
        private int _typeId;
        public int TypeId
        {
            get => _typeId;
            set => SetProperty(ref _typeId, value);
        }

        private string _typeName = string.Empty;
        public string TypeName
        {
            get => _typeName;
            set => SetProperty(ref _typeName, value);
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private int _createBy;
        public int CreateBy
        {
            get => _createBy;
            set => SetProperty(ref _createBy, value);
        }

        private string _createAt;
        public string CreateAt
        {
            get => _createAt;
            set => SetProperty(ref _createAt, value);
        }

        private int _updateBy;
        public int UpdateBy
        {
            get => _updateBy;
            set => SetProperty(ref _updateBy, value);
        }

        private string _updateAt;
        public string UpdateAt
        {
            get => _updateAt;
            set => SetProperty(ref _updateAt, value);
        }

        private ObservableCollection<NoteCategoryUiModel> _categoryList = new ObservableCollection<NoteCategoryUiModel>();
        public ObservableCollection<NoteCategoryUiModel> CategoryList
        {
            get => _categoryList;
            set => SetProperty(ref _categoryList, value);
        }
    }
}
