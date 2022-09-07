namespace ReactiveDemo.ViewModels.Adorner
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Prism.Mvvm;

    public class NameModel : BindableBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }

    public class AdornerSampleViewModel : ViewModelBase
    {
        #region ReactiveProperties

        public ObservableCollection<NameModel> NameCollection { set; get; }

        public NameModel SelectedName { set; get; }

        #endregion

        protected override void InitData()
        {
            base.InitData();

            NameCollection = new ObservableCollection<NameModel>
            {
                new NameModel {Name = "Test1"},
                new NameModel {Name = "Test2"},
                new NameModel {Name = "Test3"},
                new NameModel {Name = "Test4"}
            };

            SelectedName = NameCollection.FirstOrDefault();
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();
        }
    }
}