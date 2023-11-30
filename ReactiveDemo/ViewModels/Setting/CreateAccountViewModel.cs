using BaseDemo.Util;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveDemo.Models.Account;
using ReactiveDemo.Models.Login;
using ReactiveDemo.Models.UiModel;
using ReactiveDemo.Util.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.ViewModels.Setting
{
    public class CreateAccountViewModel : ViewModelBase
    {
        #region Field

        private AccountModel _accountModel = new AccountModel();

        #endregion

        #region PrivateProperty

        public ReactiveProperty<string> UserName { get; set; }

        public ReactiveProperty<string> Password { get; set; }

        public ReactiveProperty<string> ConfirmPassword { get; set; }

        #endregion

        #region ReactiveCommand

        public AsyncReactiveCommand CreateCommand { get; set; }

        #endregion

        #region ReactiveProperty



        #endregion

        #region Request

        

        #endregion

        #region Events



        #endregion

        #region Override

        protected override async void InitData()
        {
            base.InitData();
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();

            UserName = new ReactiveProperty<string>().AddTo(DisposablePool);
            Password = new ReactiveProperty<string>().AddTo(DisposablePool);
            ConfirmPassword = new ReactiveProperty<string>().AddTo(DisposablePool);

            UserName.SetValidateNotifyError(x => !string.IsNullOrEmpty(x) && !LoginUtil.ValidateAsciiOnly(x, "UserName", out var message) ? message : null).AddTo(DisposablePool);
            Password.SetValidateNotifyError(x => !string.IsNullOrEmpty(x) && !LoginUtil.ValidateAsciiOnly(x, "Password", out var message) ? message : null).AddTo(DisposablePool);
            ConfirmPassword.SetValidateNotifyError(x =>
            {
                if (string.IsNullOrEmpty(x)) return "Confirm password is empty!";

                if (!string.Equals(x, Password.Value)) return "Confirm password is different from password!";

                if (!LoginUtil.ValidateAsciiOnly(x, "Password", out var message)) return message;

                return null;
                
            }).AddTo(DisposablePool);
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            CreateCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            CreateCommand.Subscribe(CreateAsync).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private async Task CreateAsync()
        {
            if (!string.Equals(ConfirmPassword.Value, Password.Value)) return;

            if (!LoginModel.CheckUserInfo(UserName.Value, Password.Value)) return;

            var inserNum = await _accountModel.CreateAccountAsync(new AccountUiModel
            { 
                UserName = UserName.Value,
                Password = Password.Value
            });

            if (inserNum > 0)
            {

            }

            Close();
        }

        private void Close()
        {
            FinishInteraction?.Invoke();
        }

        #endregion
    }
}
