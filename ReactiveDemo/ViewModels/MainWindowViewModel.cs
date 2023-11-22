﻿using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Field



        #endregion

        #region PrivateProperty



        #endregion

        #region ReactiveCommand

        public ReactiveCommand ReactiveDemoCommand { get; set; }

        #endregion

        #region ReactiveProperty



        #endregion

        #region Request

        public InteractionRequest<Notification> ReactiveDemoRequest { get; set; } = new InteractionRequest<Notification>();

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

        #endregion
    }
}
