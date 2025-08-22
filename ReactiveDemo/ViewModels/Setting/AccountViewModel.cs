using Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveDemo.Models.Account;
using ReactiveDemo.Models.UiModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReactiveDemo.ViewModels.Setting
{
    public class AccountViewModel : ViewModelBase
    {

        #region Field

        private AccountModel _accountModel = new AccountModel();

        public ObservableCollection<AccountUiModel> AccountCollection { get; set; } = new ObservableCollection<AccountUiModel>();

        #endregion

        #region PrivateProperty



        #endregion

        #region ReactiveCommand

        public ReactiveCommand CreateAccountCommand { get; set; }

        public AsyncReactiveCommand<AccountUiModel> DeleteAccountCommand { get; set; }

        #endregion

        #region ReactiveProperty

        

        #endregion

        #region Request

        public InteractionRequest<Notification> CreateAccountRequest { get; set; } = new InteractionRequest<Notification>();

        #endregion

        #region Events



        #endregion

        #region Override

        protected override async void InitData()
        {
            base.InitData();

            var uiModelList = await _accountModel.LoadAccountListAsync();
            AccountCollection.AddRange(uiModelList);
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            CreateAccountCommand = new ReactiveCommand().AddTo(DisposablePool);
            CreateAccountCommand.Subscribe(CreateAccount).AddTo(DisposablePool);

            DeleteAccountCommand = new AsyncReactiveCommand<AccountUiModel>().AddTo(DisposablePool);
            DeleteAccountCommand.Subscribe(DeleteAccount).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private void CreateAccount()
        {
            CreateAccountRequest.Raise(new Notification(), async notification =>
            {
                var uiModelList = await _accountModel.LoadAccountListAsync();
                
                foreach (var uiModel in uiModelList)
                {
                    if (!AccountCollection.Any(o => string.Equals(o.UserId, uiModel.UserId)))
                    {
                        AccountCollection.Add(uiModel);
                    }
                }
            });
        }

        private async Task DeleteAccount(AccountUiModel deleteAccount)
        {
            if (deleteAccount == null) return;

            var deleteNum = await _accountModel.DeleteAccountAsync(deleteAccount);
            if (deleteNum > 0)
            {
                AccountCollection.Remove(deleteAccount);
            }
        }

        #endregion
    }
}
