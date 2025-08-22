
using Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.ViewModels.MainWindow
{
    public class HomeViewModel : ViewModelBase
    {
        #region Field



        #endregion

        #region PrivateProperty



        #endregion

        #region ReactiveCommand

        public ReactiveCommand ReactiveDemoCommand { get; set; }

        public ReactiveCommand OpenViewport3DViewCommand { get; set; }

        public ReactiveCommand OpenSearchFileByKeyWordCommand { get; set; }

        public ReactiveCommand DeepSeekDemoCommand { get; set; }

        public ReactiveCommand OpenCVCommand { get; set; }

        #endregion

        #region ReactiveProperty



        #endregion

        #region Request

        public InteractionRequest<Notification> ReactiveDemoRequest { get; set; } = new InteractionRequest<Notification>();

        public InteractionRequest<Notification> OpenViewport3DViewRequest { get; set; } = new InteractionRequest<Notification>();

        public InteractionRequest<Notification> OpenSearchFileByKeyWordRequest { get; set; } = new InteractionRequest<Notification>();

        public InteractionRequest<Notification> OpenDeepSeekDemoRequest { get; set; } = new InteractionRequest<Notification>();

        public InteractionRequest<Notification> ShowOpenCVDemoRequest { get; set; } = new InteractionRequest<Notification>();

        #endregion

        #region Events



        #endregion

        #region Override

        protected override void InitData()
        {
            base.InitData();

        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            ReactiveDemoCommand = new ReactiveCommand().AddTo(DisposablePool);
            ReactiveDemoCommand.Subscribe(ReactiveDemoMethod).AddTo(DisposablePool);

            OpenViewport3DViewCommand = new ReactiveCommand().AddTo(DisposablePool);
            OpenViewport3DViewCommand.Subscribe(OpenViewport3DView).AddTo(DisposablePool);

            OpenSearchFileByKeyWordCommand = new ReactiveCommand().AddTo(DisposablePool);
            OpenSearchFileByKeyWordCommand.Subscribe(OpenSearchFileByKeyWord).AddTo(DisposablePool);

            DeepSeekDemoCommand = new ReactiveCommand().AddTo(DisposablePool);
            DeepSeekDemoCommand.Subscribe(DeepSeekDemo).AddTo(DisposablePool);

            OpenCVCommand = new ReactiveCommand().AddTo(DisposablePool);
            OpenCVCommand.Subscribe(OpenCVDemo).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private void ReactiveDemoMethod()
        {
            ReactiveDemoRequest.Raise(new Notification(), notification => { });
        }

        private void OpenViewport3DView()
        {
            OpenViewport3DViewRequest.Raise(new Notification(), notification => { });
        }

        private void OpenSearchFileByKeyWord()
        {
            OpenSearchFileByKeyWordRequest.Raise(new Notification(), notification => { });
        }

        private void DeepSeekDemo()
        {
            OpenDeepSeekDemoRequest.Raise(new Notification(), notification => { });
        }

        private void OpenCVDemo()
        {
            ShowOpenCVDemoRequest.Raise(new Notification(), notifiction => { });
        }

        #endregion
    }
}
