namespace ReactiveDemo.ViewModels
{
    using Events;
    using Reactive.Bindings;
    using Reactive.Bindings.Extensions;

    public class Window1ViewModel : ViewModelBase
    {
        #region ReactiveCommand

        public ReactiveCommand CloseWindowCommand { get; set; }

        #endregion

        protected override void InitData()
        {
            base.InitData();

            EventAggregator.GetEvent<ReactiveDemoEvents.Test1>().Publish(1);
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            CloseWindowCommand = new ReactiveCommand().AddTo(DisposablePool);
            CloseWindowCommand.Subscribe(CloseWindow).AddTo(DisposablePool);
        }

        #region Functions

        private void CloseWindow()
        {
            FinishInteractionCommand.Execute();
        }

        #endregion
    }
}