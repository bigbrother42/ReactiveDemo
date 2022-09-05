namespace ReactiveDemo.ViewModels
{
    using System.Reactive.Disposables;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
    using Models;
    using Reactive.Bindings;
    using Reactive.Bindings.Extensions;

    public class ReactiveDemoViewModel
    {
        public CompositeDisposable DisposablePool { get; }

        public ReactiveCommand OpenWindow1Command { get; set; }

        public ReactiveProperty<string> Button1Name { get; set; }

        public InteractionRequest<Notification> LoginOpenPageRequest { get; set; }

        private ReactiveDemoModel _reactiveDemoModel;

        public ReactiveDemoViewModel()
        {
            DisposablePool = new CompositeDisposable();

            OpenWindow1Command = new ReactiveCommand().AddTo(DisposablePool);
            OpenWindow1Command.Subscribe(OpenWindow1).AddTo(DisposablePool);

            Button1Name = new ReactiveProperty<string>().AddTo(DisposablePool);
            Button1Name.Value = "Window1";

            LoginOpenPageRequest = new InteractionRequest<Notification>();

            _reactiveDemoModel = new ReactiveDemoModel();
        }

        ~ReactiveDemoViewModel()
        {
            DisposablePool.Dispose();
        }

        #region Functions

        private void OpenWindow1()
        {
            LoginOpenPageRequest.Raise(new Notification(), OpenWindow1Callback);
        }

        private void OpenWindow1Callback(Notification obj)
        {
            
        }

        #endregion
    }
}