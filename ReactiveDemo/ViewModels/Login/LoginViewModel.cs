using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using SharedDemo.GlobalData;
using ReactiveDemo.Models.Login;

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

        public ReactiveCommand LoginCommand { get; set; }

        #endregion

        #region ReactiveProperty

        public ReactiveProperty<string> UserName { get; set; }

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
            UserName.Value = "guobin";
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
            _loginModel.Login(new DataDemo.WebDto.UserBasicInfoWebDto
            { 
                UserName = UserName.Value
            });

            MainWindowRequest.Raise(new Notification(), notification => { });
        }

        #endregion
    }
}
