namespace ReactiveDemo.ViewModels
{
    using Events;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
    using Reactive.Bindings;
    using Reactive.Bindings.Extensions;

    public class Window1ViewModel : ViewModelBase
    {
        #region ReactiveCommand

        public ReactiveCommand CloseWindowCommand { get; set; }

        public ReactiveCommand AdornerSampleCommand { get; set; }

        public ReactiveCommand VisualSampleCommand { get; set; }

        #endregion

        #region Requests

        public InteractionRequest<Notification> AdornerSampleRequest { get; set; } = new InteractionRequest<Notification>();

        public InteractionRequest<Notification> VisualSampleRequest { get; set; } = new InteractionRequest<Notification>();

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

            AdornerSampleCommand = new ReactiveCommand().AddTo(DisposablePool);
            AdornerSampleCommand.Subscribe(AdornerSample).AddTo(DisposablePool);

            VisualSampleCommand = new ReactiveCommand().AddTo(DisposablePool);
            VisualSampleCommand.Subscribe(VisualSample).AddTo(DisposablePool);
        }

        #region Functions

        private void CloseWindow()
        {
            FinishInteractionCommand.Execute();
        }

        private void AdornerSample()
        {
            AdornerSampleRequest.Raise(new Notification());
        }

        private void VisualSample()
        {
            VisualSampleRequest.Raise(new Notification());
        }

        #endregion
    }
}