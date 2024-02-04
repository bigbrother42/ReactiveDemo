using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Prism.Interactivity.InteractionRequest;
using SharedDemo.GlobalData;
using ReactiveDemo.Models.Login;
using System.Text.RegularExpressions;
using System;
using System.Threading.Tasks;
using BaseDemo.Util;
using ReactiveDemo.Util.Login;
using static InfrastructureDemo.Constants.Constants;
using DataDemo.WebDto;
using ReactiveDemo.Base.ActionBase;
using System.Windows.Threading;

namespace ReactiveDemo.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        #region Field

        

        #endregion

        #region PrivateProperty

        private readonly SqLiteDbContext _dbContext = new SqLiteDbContext(GlobalData.DbConnection);

        private LoginModel _loginModel;

        private DispatcherTimer _dispatcherTimer { get; set; } = new DispatcherTimer();

        #endregion

        #region ReactiveCommand

        public AsyncReactiveCommand LoginCommand { get; set; }

        #endregion

        #region ReactiveProperty

        public ReactiveProperty<string> UserName { get; set; }

        public ReactiveProperty<string> Password { get; set; }

        public ReactiveProperty<bool> IsPasswordErrorPopupShow { get; set; }

        #endregion

        #region Request

        public InteractionRequest<Notification> MainWindowRequest { get; set; } = new InteractionRequest<Notification>();

        public InteractionRequest<MethodNotification> FocusPasswordTextBoxRequest { get; set; } = new InteractionRequest<MethodNotification>();

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

            IsPasswordErrorPopupShow = new ReactiveProperty<bool>().AddTo(DisposablePool);
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
                // password invalid
                FocusPasswordTextBoxRequest.Raise(new MethodNotification());
                IsPasswordErrorPopupShow.Value = true;
                _dispatcherTimer.Tick += ClosePasswordValidPopup;
                _dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
                _dispatcherTimer.Start();

                return;
            }

            _loginModel.SetSystemTheme();

            MainWindowRequest.Raise(new Notification(), notification => { });
        }

        public void ClosePasswordValidPopup(object sender, EventArgs e)
        {
            IsPasswordErrorPopupShow.Value = false;
            _dispatcherTimer.Stop();
        }

        #endregion
    }
}
