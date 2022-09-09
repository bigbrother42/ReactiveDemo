namespace ReactiveDemo.ViewModels.Visual
{
    using Reactive.Bindings;

    public class TestModel
    {
        public string Code;

        public string Name;
    }

    public class VisualViewModel : ViewModelBase
    {
        #region ReactiveProperty



        #endregion

        #region ReactiveCommand

        public ReactiveCollection<TestModel> ComboBoxCollection { get; set; }

        #endregion

        protected override void InitData()
        {
            base.InitData();

            ComboBoxCollection = new ReactiveCollection<TestModel>();
            for (var i = 0; i < 20000; i++)
            {
                ComboBoxCollection.Add(new TestModel
                {
                    Code = i + "",
                    Name = "test"+ i
                });
            }
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();
        }
    }
}