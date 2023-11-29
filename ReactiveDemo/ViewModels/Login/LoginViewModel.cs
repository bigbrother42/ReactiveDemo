using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using SharedDemo.GlobalData;
using ReactiveDemo.Models.Login;
using System.Text.RegularExpressions;
using System;
using System.Threading.Tasks;

namespace ReactiveDemo.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        #region Field

        private static readonly string VALIDATION_ERROR_ASCIIONLY = "Please enter {0} using alphanumeric characters or symbols.";

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

        public ReactiveProperty<bool> CanUserLogin { get; set; }

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
            CanUserLogin = new ReactiveProperty<bool>().AddTo(DisposablePool);

            UserName.SetValidateNotifyError(x => !string.IsNullOrEmpty(x) && !ValidateAsciiOnly(x, "UserName", out var message) ? message : null)
                .Subscribe(code => {
                    CanUserLogin.Value = !string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(Password.Value) && StringIsAsciiOnly(code) && StringIsAsciiOnly(Password.Value);
                })
                .AddTo(DisposablePool);

            Password.SetValidateNotifyError(x => !string.IsNullOrEmpty(x) && !ValidateAsciiOnly(x, "Password", out var message) ? message : null)
                .Subscribe(pass =>
                {
                    CanUserLogin.Value = !string.IsNullOrWhiteSpace(pass) && !string.IsNullOrWhiteSpace(UserName.Value) && StringIsAsciiOnly(pass) && StringIsAsciiOnly(UserName.Value);
                })
                .AddTo(DisposablePool);
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
            if (string.IsNullOrEmpty(UserName.Value))
            {
                UserName.ForceValidate();
            }

            if (string.IsNullOrEmpty(Password.Value))
            {
                Password.ForceValidate();
            }

            if (!CanUserLogin.Value) return;

            var isSuccess = true;
            if (!string.Equals(UserName.Value, "admin"))
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
                isSuccess = string.Equals(Password.Value, "admin");
            }

            if (!isSuccess)
            {
                return;
            }

            MainWindowRequest.Raise(new Notification(), notification => { });
        }

        private bool StringIsAsciiOnly(string str)
        {
            return Regex.IsMatch(str, @"^[\x00-\x7F]+$");
        }

        private bool ValidateAsciiOnly(string str, string caption, out string message)
        {
            var validationResult = StringIsAsciiOnly(str);
            if (!validationResult)
            {
                message = string.Format(VALIDATION_ERROR_ASCIIONLY, caption);
            }
            else
            {
                message = null;
            }
            return validationResult;
        }

        #endregion
    }
}
