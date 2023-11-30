using DataDemo.WebDto;
using ServiceDemo.Common.SQLite;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseDemo.Util.Extensions;
using BaseDemo.Util;
using ReactiveDemo.Util.Login;

namespace ReactiveDemo.Models.Login
{
    public class LoginModel
    {
        private UserBasicInfoService _userBasicInfoService = new UserBasicInfoService();

        private readonly SqLiteDbContext _dbContext = new SqLiteDbContext(GlobalData.DbConnection);

        public async Task<bool> LoginAsync(UserBasicInfoWebDto userBasicInfoWebDto)
        {
            var userList = await _userBasicInfoService.SelectUserBasicInfoListAsync(_dbContext, userBasicInfoWebDto);

            if (userList.IsNullOrEmpty()) return false;

            var dbUser = userList.First();

            if (!string.Equals(dbUser.Password, userBasicInfoWebDto.Password)) return false;

            LoginInfo.UserBasicInfo = dbUser;

            return true;
        }

        public static bool CheckUserInfo(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)) return false;

            if (string.IsNullOrEmpty(password)) return false;

            if (!LoginUtil.StringIsAsciiOnly(userName)) return false;

            if (!LoginUtil.StringIsAsciiOnly(password)) return false;


            return true;
        }
    }
}
