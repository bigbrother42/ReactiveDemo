using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using SharedDemo.GlobalData;
using ReactiveDemo.Models.Login;
using System.Text.RegularExpressions;
using System;
using System.Threading.Tasks;
using BaseDemo.Util;
using ReactiveDemo.Util.Login;
using static InfrastructureDemo.Constants.Constants;
using DataDemo.WebDto;

namespace ReactiveDemo.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        #region Field

        

        #endregion

        #region PrivateProperty

        private readonly SqLiteDbContext _dbContext = new SqLiteDbContext(GlobalData.DbConnection);

        private LoginModel _loginModel;

        #endregion

        #region ReactiveCommand

        public AsyncReactiveCommand LoginCommand { get; set; }

        #endregion

        #region ReactiveProperty

        public ReactiveProperty<string> UserName { get; set; }

        public ReactiveProperty<string> Password { get; set; }

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

            _loginModel = new LoginModel();
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();

            UserName = new ReactiveProperty<string>().AddTo(DisposablePool);
            Password = new ReactiveProperty<string>().AddTo(DisposablePool);

            UserName.SetValidateNotifyError(x => !string.IsNullOrEmpty(x) && !LoginUtil.ValidateAsciiOnly(x, "UserName", out var message) ? message : null).AddTo(DisposablePool);
            Password.SetValidateNotifyError(x => !string.IsNullOrEmpty(x) && !LoginUtil.ValidateAsciiOnly(x, "Password", out var message) ? message : null).AddTo(DisposablePool);
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            LoginCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            LoginCommand.Subscribe(LoginAsync).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private async Task LoginAsync()
        {
            if (!LoginModel.CheckUserInfo(UserName.Value, Password.Value)) return;

            var isSuccess = true;
            if (!string.Equals(UserName.Value, LoginConstants.USERNAME_ADMIN))
            {
                isSuccess = await _loginModel.LoginAsync(new DataDemo.WebDto.UserBasicInfoWebDto
                {
                    UserName = UserName.Value,
                    Password = Password.Value
                });
            }
            else
            {
                // when username is admin
                isSuccess = string.Equals(Password.Value, LoginConstants.USERNAME_ADMIN);

                LoginInfo.IsAdmin = true;
                LoginInfo.UserBasicInfo = new UserBasicInfoWebDto
                {
                    UserId = 0,
                    UserName = "admin"
                };
            }

            if (!isSuccess)
            {
                return;
            }

            MainWindowRequest.Raise(new Notification(), notification => { });
        }

        #endregion
    }
}
