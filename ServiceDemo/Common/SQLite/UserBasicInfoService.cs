using DataDemo.WebDto;
using ServiceDemo.Base;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using SharedDemo.Util;
using System.Data;

namespace ServiceDemo.Common.SQLite
{
    public class UserBasicInfoService : ClientApiService
    {
        public List<UserBasicInfoWebDto> SelectUserBasicInfoList(SqLiteDbContext dbContext, UserBasicInfoWebDto param)
        {
            var userList = new List<UserBasicInfoWebDto>();

            var sql = @"SELECT 
                         ubi.UserId,
                         ubi.UserName, 
                         ubi.Password,
                         ubi.StartDate,
                         ubi.EndDate,
                         ubi.CreateBy,
                         ubi.CreateAt,
                         ubi.UpdateBy,
                         ubi.UpdateAt
                        FROM user_basic_info ubi
                        WHERE ubi.UserName = @UserName";

            var userModelList = dbContext.Database.ExecuteRawSqlQueryAutoMapper<UserBasicInfoWebDto>(sql, dbCommand =>
            {
                var userNameParam = dbCommand.CreateParameter();
                userNameParam.ParameterName = "@UserName";
                userNameParam.DbType = DbType.String;
                userNameParam.Value = param.UserName;
                dbCommand.Parameters.Add(userNameParam);
            });

            if (userModelList != null && userModelList.Count > 0)
            {
                userList.AddRange(userModelList);
            }

            return userList;
        }
    }
}
