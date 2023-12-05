using BaseDemo.Util.Extensions;
using DataDemo.WebDto;
using ReactiveDemo.Models.UiModel;
using ServiceDemo.Common.SQLite;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Models.Account
{
    public class AccountModel
    {
        private UserBasicInfoService _userBasicInfoService = new UserBasicInfoService();

        public async Task<List<AccountUiModel>> LoadAccountListAsync()
        {
            var acountList = new List<AccountUiModel>();

            var userList = await _userBasicInfoService.SelectAllUserBasicInfoListAsync();
            if (!userList.IsNullOrEmpty())
            {
                foreach (var user in userList)
                {
                    acountList.Add(new AccountUiModel
                    { 
                        UserId = user.UserId.ToString(),
                        UserName = user.UserName,
                        StartDate = user.StartDate,
                        EndDate = user.EndDate
                    });
                }
            }

            return acountList;
        }

        public async Task<int> CreateAccountAsync(AccountUiModel accountUiModel)
        {
            var param = new UserBasicInfoWebDto
            {
                UserName = accountUiModel.UserName,
                Password = accountUiModel.Password,
                StartDate = DateTime.Now,
                EndDate = DateTime.MaxValue,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                CreateBy = 1,
                UpdateBy = 1
            };

            var insertNum = await _userBasicInfoService.InsertAccount(param);

            return insertNum;
        }

        public async Task<int> DeleteAccountAsync(AccountUiModel accountUiModel)
        {
            var param = new UserBasicInfoWebDto
            {
                UserId = int.Parse(accountUiModel.UserId),
                UserName = accountUiModel.UserName,
                Password = accountUiModel.Password,
                StartDate = DateTime.Now,
                EndDate = DateTime.MaxValue,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                CreateBy = 1,
                UpdateBy = 1
            };

            var insertNum = await _userBasicInfoService.DeleteAccount(param);

            return insertNum;
        }

        public async Task<bool> CheckUserIsExist(string userName)
        {
            var userList = await _userBasicInfoService.SelectUserBasicInfoListAsync(new UserBasicInfoWebDto 
            { 
                UserName = userName
            });

            if (!userList.IsNullOrEmpty()) return true;

            return false;
        }
    }
}
