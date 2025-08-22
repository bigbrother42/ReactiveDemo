
using Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static InfrastructureDemo.Constants.Constants;

namespace ReactiveDemo.ViewModels.MainWindow
{
    public class PrivateViewModel : ViewModelBase
    {
        #region Field



        #endregion

        #region PrivateProperty



        #endregion

        #region ReactiveCommand

        public ReactiveCommand AccountViewCommand { get; set; }

        #endregion

        #region ReactiveProperty

        public ReactiveProperty<Visibility> IsAccountSettingShow { get; set; }

        #endregion

        #region Request

        public InteractionRequest<Notification> AccountViewRequest { get; set; } = new InteractionRequest<Notification>();

        #endregion

        #region Events



        #endregion

        #region Override

        protected override void InitData()
        {
            base.InitData();

            IsAccountSettingShow.Value = LoginInfo.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();

            IsAccountSettingShow = new ReactiveProperty<Visibility>(Visibility.Collapsed).AddTo(DisposablePool);
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            AccountViewCommand = new ReactiveCommand().AddTo(DisposablePool);
            AccountViewCommand.Subscribe(AccountView).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private void AccountView()
        {
            AccountViewRequest.Raise(new Notification(), notification => { });
        }

        #endregion
    }
}
