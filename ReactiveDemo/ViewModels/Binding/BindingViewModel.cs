using System.Threading;
using System.Threading.Tasks;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace ReactiveDemo.ViewModels.Binding
{
    public class BindingViewModel : ViewModelBase
    {
        #region Models



        #endregion

        #region Properties

        private Task<int> _testTask;

        public Task<int> TestTask
        {
            get => _testTask;
            set => SetProperty(ref value, _testTask);
        }

        #endregion

        #region ReactiveProperty



        #endregion

        #region ReactiveCommand

        public AsyncReactiveCommand RequestValueCommand { get; set; }

        #endregion

        #region Request



        #endregion

        #region Override

        protected override void InitData()
        {
            base.InitData();
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            RequestValueCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            RequestValueCommand.Subscribe(RequestValueMethod).AddTo(DisposablePool);
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Functions

        private async Task RequestValueMethod()
        {
            await Task.Delay(5000);

            TestTask = new Task<int>(() =>
            {
                return 50;
            });
        }

        #endregion
    }
}