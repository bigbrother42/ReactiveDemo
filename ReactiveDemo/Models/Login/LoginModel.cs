using DataDemo.WebDto;
using ServiceDemo.Common.SQLite;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseDemo.Util.Extensions;

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

            return string.Equals(dbUser.Password, userBasicInfoWebDto.Password);
        }
    }
}
