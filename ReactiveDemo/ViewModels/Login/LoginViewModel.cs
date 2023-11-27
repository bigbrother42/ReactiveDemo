﻿using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace ReactiveDemo.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        #region Field



        #endregion

        #region PrivateProperty



        #endregion

        #region ReactiveCommand

        public ReactiveCommand LoginCommand { get; set; }

        #endregion

        #region ReactiveProperty



        #endregion

        #region Request

        public InteractionRequest<Notification> MainWindowRequest { get; set; } = new InteractionRequest<Notification>();

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

            LoginCommand = new ReactiveCommand().AddTo(DisposablePool);
            LoginCommand.Subscribe(Login).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private void Login()
        {
            MainWindowRequest.Raise(new Notification(), notification => { });
        }

        #endregion
    }
}