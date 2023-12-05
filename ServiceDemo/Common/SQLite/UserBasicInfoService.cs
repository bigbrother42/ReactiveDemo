using DataDemo.WebDto;
using ServiceDemo.Base;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using SharedDemo.Util;
using System.Data;
using System.Threading.Tasks;
using BaseDemo.Util.Extensions;

namespace ServiceDemo.Common.SQLite
{
    public class UserBasicInfoService : ClientApiService
    {
        public async Task<List<UserBasicInfoWebDto>> SelectUserBasicInfoListAsync(UserBasicInfoWebDto param)
        {
            var userList = new List<UserBasicInfoWebDto>();

            var taskResult = await Task.Run(() => 
            {
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
                            FROM UserBasicInfo ubi
                            WHERE ubi.UserName = @UserName";

                var userModelList = SqLiteDbContext.Database.ExecuteRawSqlQueryAutoMapper<UserBasicInfoWebDto>(sql, dbCommand =>
                {
                    var userNameParam = dbCommand.CreateParameter();
                    userNameParam.ParameterName = "@UserName";
                    userNameParam.DbType = DbType.String;
                    userNameParam.Value = param.UserName;
                    dbCommand.Parameters.Add(userNameParam);
                });

                return userModelList;
            });

            if (!taskResult.IsNullOrEmpty())
            {
                userList.AddRange(taskResult);
            }

            return userList;
        }

        public async Task<List<UserBasicInfoWebDto>> SelectAllUserBasicInfoListAsync()
        {
            var userList = new List<UserBasicInfoWebDto>();

            var taskResult = await Task.Run(() =>
            {
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
                            FROM UserBasicInfo ubi";

                var userModelList = SqLiteDbContext.Database.ExecuteRawSqlQueryAutoMapper<UserBasicInfoWebDto>(sql);

                return userModelList;
            });

            if (!taskResult.IsNullOrEmpty())
            {
                userList.AddRange(taskResult);
            }

            return userList;
        }

        public async Task<int> InsertAccount(UserBasicInfoWebDto param)
        {
            if (param == null) return 0;

            SqLiteDbContext.UserBasicInfo.Add(param);

            var taskResult = await Task.Run(() =>
            {
                return SqLiteDbContext.SaveChanges();
            });

            return taskResult;
        }

        public async Task<int> DeleteAccount(UserBasicInfoWebDto param)
        {
            if (param == null) return 0;

            var deleteItem = SqLiteDbContext.UserBasicInfo.Find(param.UserId);

            if (deleteItem != null)
            {
                SqLiteDbContext.UserBasicInfo.Remove(deleteItem);

                var taskResult = await Task.Run(() =>
                {
                    return SqLiteDbContext.SaveChanges();
                });

                return taskResult;
            }

            return 0;
        }
    }
}
