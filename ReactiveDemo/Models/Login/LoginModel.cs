using DataDemo.WebDto;
using ServiceDemo.Common.SQLite;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Models.Login
{
    public class LoginModel
    {
        private UserBasicInfoService _userBasicInfoService = new UserBasicInfoService();

        private readonly SqLiteDbContext _dbContext = new SqLiteDbContext(GlobalData.DbConnection);

        public void Login(UserBasicInfoWebDto userBasicInfoWebDto)
        {
            var userList = _userBasicInfoService.SelectUserBasicInfoList(_dbContext, userBasicInfoWebDto);
        }
    }
}
