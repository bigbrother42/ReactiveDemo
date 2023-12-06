using BaseDemo.Util.Extensions;
using DataDemo.WebDto;
using DataDemo.WebDto.Custom;
using ServiceDemo.Base;
using SharedDemo.GlobalData;
using SharedDemo.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDemo.Common.SQLite
{
    public class NoteService : ClientApiService
    {
        public async Task<NoteCategoryWebDto> InsertOrUpdateNoteCategoryAsync(NoteCategoryWebDto param)
        {
            if (param == null) return null;

            var existCategoryList = await SelectNoteCategoryListAsync(param);

            if (existCategoryList.IsNullOrEmpty())
            {
                // insert
                SqLiteDbContext.NoteCategory.Add(param);

                var taskResult = await Task.Run(() =>
                {
                    SqLiteDbContext.SaveChanges();

                    return param;
                });
            }
            else
            {
                // update
                var taskResult = await Task.Run(() =>
                {
                    var existItem = SqLiteDbContext.NoteCategory.FirstOrDefault(o => o.CategoryId == param.CategoryId);
                    if (existItem != null)
                    {
                        existItem.CategoryName = param.CategoryName;
                        SqLiteDbContext.SaveChanges();

                        return param;
                    }
                    else
                    {
                        return null;
                    }
                });
            }

            return null;
        }

        public async Task<NoteContentWebDto> InsertOrUpdateNoteContentAsync(NoteContentWebDto param)
        {
            if (param == null) return null;

            var existContent = SqLiteDbContext.NoteContent.FirstOrDefault(o => o.CategoryId == param.CategoryId);
            if (existContent == null)
            {
                // insert
                SqLiteDbContext.NoteContent.Add(param);
            }
            else
            {
                // update
                existContent.Content = param.Content;
            }

            await Task.Run(() =>
            {
                SqLiteDbContext.SaveChanges();

                return param;
            });

            return null;
        }

        public async Task<int> DeleteNoteCategoryAsync(NoteCategoryWebDto param)
        {
            if (param == null) return 0;

            var deleteItem = SqLiteDbContext.NoteCategory.Find(param.CategoryId);

            if (deleteItem != null)
            {
                SqLiteDbContext.NoteCategory.Remove(deleteItem);

                var taskResult = await Task.Run(() =>
                {
                    return SqLiteDbContext.SaveChanges();
                });

                return taskResult;
            }

            return 0;
        }

        public async Task<List<NoteCategoryWebDto>> SelectNoteCategoryListAsync(NoteCategoryWebDto param)
        {
            if (param == null) return new List<NoteCategoryWebDto>(); 

            var noteCategoryList = new List<NoteCategoryWebDto>();

            var taskResult = await Task.Run(() =>
            {
                var sql = @"SELECT 
                             nc.CategoryId,
                             nc.CategoryName, 
                             nc.CreateBy,
                             nc.CreateAt,
                             nc.UpdateBy,
                             nc.UpdateAt
                            FROM NoteCategory nc
                            WHERE nc.CategoryId = @CategoryId";

                var noteCategoryModelList = SqLiteDbContext.Database.ExecuteRawSqlQueryAutoMapper<NoteCategoryWebDto>(sql, dbCommand =>
                {
                    var userNameParam = dbCommand.CreateParameter();
                    userNameParam.ParameterName = "@CategoryId";
                    userNameParam.DbType = DbType.Int32;
                    userNameParam.Value = param.CategoryId;
                    dbCommand.Parameters.Add(userNameParam);
                });

                return noteCategoryModelList;
            });

            if (!taskResult.IsNullOrEmpty())
            {
                noteCategoryList.AddRange(taskResult);
            }

            return noteCategoryList;
        }

        public async Task<List<NoteCategoryCustomWebDto>> SelectAllNoteCategoryListAsync()
        {
            var noteCategoryList = new List<NoteCategoryCustomWebDto>();

            var taskResult = await Task.Run(() =>
            {
                var sql = @"SELECT 
                             nc.CategoryId,
                             nc.CategoryName, 
                             nc2.ContentId,
                             nc2.Content
                            FROM NoteCategory nc
                            LEFT JOIN NoteContent nc2
                            ON nc.CategoryId = nc2.CategoryId";

                var noteCategoryModelList = SqLiteDbContext.Database.ExecuteRawSqlQueryAutoMapper<NoteCategoryCustomWebDto>(sql);

                return noteCategoryModelList;
            });

            if (!taskResult.IsNullOrEmpty())
            {
                noteCategoryList.AddRange(taskResult);
            }

            return noteCategoryList;
        }
    }
}
