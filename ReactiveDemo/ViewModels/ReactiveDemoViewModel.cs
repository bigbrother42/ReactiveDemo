namespace ReactiveDemo.ViewModels
{
    using System;
    using System.Reactive.Disposables;
    using Events;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
    using Models;
    using Reactive.Bindings;
    using Reactive.Bindings.Extensions;

    public class ReactiveDemoViewModel : ViewModelBase
    {
        #region ReactiveCommand

        public ReactiveCommand OpenWindow1Command { get; set; }

        public ReactiveCommand ReactiveXZipCommand { get; set; }

        public ReactiveCommand ReactiveXSampleCommand { get; set; }

        public ReactiveCommand ReactiveXMergeCommand { get; set; }

        #endregion

        #region ReactiveProperty

        public ReactiveProperty<string> Button1Name { get; set; }

        #endregion

        #region Requests

        public InteractionRequest<Notification> LoginOpenPageRequest { get; set; } = new InteractionRequest<Notification>();

        #endregion

        #region Private Properties

        private ReactiveDemoModel _reactiveDemoModel;

        #endregion

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();

            SubscribeEvent<ReactiveDemoEvents.Test1, int>(o =>
            {
                Console.WriteLine(@"event test success!");
            });
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();

            Button1Name = new ReactiveProperty<string>().AddTo(DisposablePool);
            Button1Name.Value = "SampleView";
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            OpenWindow1Command = new ReactiveCommand().AddTo(DisposablePool);
            OpenWindow1Command.Subscribe(OpenWindow1).AddTo(DisposablePool);

            // ReactiveX-Zip
            // http://introtorx.com/Content/v1.0.10621.0/12_CombiningSequences.html#Zip
            ReactiveXZipCommand = new ReactiveCommand().AddTo(DisposablePool);
            ReactiveXZipCommand.Subscribe(ReactiveXZip).AddTo(DisposablePool);

            // ReactiveX-Sample
            // http://introtorx.com/Content/v1.0.10621.0/13_TimeShiftedSequences.html#Sample
            ReactiveXSampleCommand = new ReactiveCommand().AddTo(DisposablePool);
            ReactiveXSampleCommand.Subscribe(ReactiveXSample).AddTo(DisposablePool);

            // ReactiveX-Merge
            // http://introtorx.com/Content/v1.0.10621.0/12_CombiningSequences.html#Merge
            ReactiveXMergeCommand = new ReactiveCommand().AddTo(DisposablePool);
            ReactiveXMergeCommand.Subscribe(ReactiveXMerge).AddTo(DisposablePool);
        }

        protected override void InitData()
        {
            base.InitData();

            _reactiveDemoModel = new ReactiveDemoModel();
        }

        #region Functions

        private void ReactiveXMerge()
        {
            _reactiveDemoModel.TestReactiveXMerge();
        }

        private void ReactiveXSample()
        {
            _reactiveDemoModel.TestReactiveXSample();
        }

        private void ReactiveXZip()
        {
            _reactiveDemoModel.TestReactiveXZip();
        }

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