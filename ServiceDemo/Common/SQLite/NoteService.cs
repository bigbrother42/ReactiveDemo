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

            var existCategoryItem = SqLiteDbContext.NoteCategory.FirstOrDefault(o => o.TypeId == param.TypeId && o.CategoryId == param.CategoryId);

            if (existCategoryItem == null)
            {
                // insert
                await SqLiteDbContext.NoteCategory.AddAsync(param);
            }
            else
            {
                // update
                var existItem = SqLiteDbContext.NoteCategory.FirstOrDefault(o => o.TypeId == param.TypeId && o.CategoryId == param.CategoryId);
                if (existItem != null)
                {
                    existItem.CategoryName = param.CategoryName;
                }
            }

            await SqLiteDbContext.SaveChangesAsync();

            return param;
        }

        public async Task<NoteContentWebDto> InsertOrUpdateNoteContentAsync(NoteContentWebDto param)
        {
            if (param == null) return null;

            var existContent = SqLiteDbContext.NoteContent.FirstOrDefault(o => o.TypeId == param.TypeId && o.CategoryId == param.CategoryId);
            if (existContent == null)
            {
                // insert
                await SqLiteDbContext.NoteContent.AddAsync(param);
            }
            else
            {
                // update
                existContent.Content = param.Content;
            }

            await SqLiteDbContext.SaveChangesAsync();

            return param;
        }

        public async Task<List<NoteTypeWebDto>> InsertOrUpdateNoteTypeListAsync(List<NoteTypeWebDto> paramList)
        {
            if (paramList.IsNullOrEmpty()) return new List<NoteTypeWebDto>();

            foreach (var param in paramList)
            {
                var existContent = SqLiteDbContext.NoteType.FirstOrDefault(o => o.TypeId == param.TypeId);
                if (existContent == null)
                {
                    // insert
                    await SqLiteDbContext.NoteType.AddAsync(param);
                }
                else
                {
                    // update
                    existContent.TypeName = param.TypeName;
                    existContent.Description = param.Description;
                }
            }

            await SqLiteDbContext.SaveChangesAsync();

            return paramList;
        }

        public async Task<int> DeleteNoteTypeListAsync(List<NoteTypeWebDto> paramList)
        {
            if (paramList.IsNullOrEmpty()) return 0;

            foreach(var param in paramList)
            {
                var existContent = SqLiteDbContext.NoteType.FirstOrDefault(o => o.TypeId == param.TypeId);
                if (existContent != null)
                {
                    SqLiteDbContext.NoteType.Remove(existContent);
                }
            }

            return await SqLiteDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteNoteCategoryAsync(NoteCategoryWebDto param)
        {
            if (param == null) return 0;

            var deleteItem = SqLiteDbContext.NoteCategory.FirstOrDefault(o => o.TypeId == param.TypeId && o.CategoryId == param.CategoryId);

            if (deleteItem != null)
            {
                SqLiteDbContext.NoteCategory.Remove(deleteItem);

                // remove content
                var deleteContentItemList = SqLiteDbContext.NoteContent.Where(o => o.TypeId == deleteItem.TypeId && o.CategoryId == deleteItem.CategoryId);
                if (!deleteContentItemList.IsNullOrEmpty())
                {
                    SqLiteDbContext.NoteContent.RemoveRange(deleteContentItemList);
                }

                return await SqLiteDbContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<List<NoteCategoryCustomWebDto>> SelectAllNoteCategoryListAsync()
        {
            var noteCategoryList = new List<NoteCategoryCustomWebDto>();

            var taskResult = await Task.Run(() =>
            {
                var sql = @"SELECT 
                             nt.UserId,
                             nt.TypeId,
                             nt.TypeName,
                             nc.CategoryId,
                             nc.CategoryName, 
                             nc.DisplayOrder AS CategoryDisplayOrder,
                             nc2.ContentId,
                             nc2.Content
                            FROM NoteType nt
                            LEFT JOIN NoteCategory nc
                            ON nc.UserId = nt.UserId
                            AND nc.TypeId = nt.TypeId
                            LEFT JOIN NoteContent nc2
                            ON nc.UserId = nc2.UserId
                            AND nc.CategoryId = nc2.CategoryId
                            AND nc.TypeId = nc2.TypeId
                            WHERE nt.UserId = @UserId";

                var noteCategoryModelList = SqLiteDbContext.Database.ExecuteRawSqlQueryAutoMapper<NoteCategoryCustomWebDto>(sql, dbCommand =>
                {
                    var userIdParam = dbCommand.CreateParameter();
                    userIdParam.ParameterName = "@UserId";
                    userIdParam.DbType = DbType.Int32;
                    userIdParam.Value =LoginInfo.UserBasicInfo.UserId;
                    dbCommand.Parameters.Add(userIdParam);
                });

                return noteCategoryModelList;
            });

            if (!taskResult.IsNullOrEmpty())
            {
                noteCategoryList.AddRange(taskResult);
            }

            return noteCategoryList;
        }

        public async Task<List<NoteTypeWebDto>> SelectAllNoteTypeListAsync()
        {
            var noteTypeList = new List<NoteTypeWebDto>();

            var taskResult = await Task.Run(() =>
            {
                var sql = @"SELECT 
                             nt.UserId,
                             nt.TypeId,
                             nt.TypeName, 
                             nt.Description,
                             nt.CreateBy,
                             nt.CreateAt,
                             nt.UpdateBy,
                             nt.UpdateAt
                            FROM NoteType nt";

                var noteTypeModelList = SqLiteDbContext.Database.ExecuteRawSqlQueryAutoMapper<NoteTypeWebDto>(sql);

                return noteTypeModelList;
            });

            if (!taskResult.IsNullOrEmpty())
            {
                noteTypeList.AddRange(taskResult);
            }

            return noteTypeList;
        }

        public async Task UpdateCategoryDisplayOrderAsync(List<NoteCategoryWebDto> noteCategoryWebDtoList)
        {
            if (noteCategoryWebDtoList.IsNullOrEmpty()) return;

            foreach (var noteCategoryWebDto in noteCategoryWebDtoList)
            {
                var existItem = SqLiteDbContext.NoteCategory.FirstOrDefault(o => o.UserId == noteCategoryWebDto.UserId
                    && o.TypeId == noteCategoryWebDto.TypeId
                    && o.CategoryId == noteCategoryWebDto.CategoryId);

                if (existItem != null)
                {
                    existItem.DisplayOrder = noteCategoryWebDto.DisplayOrder;
                }
            }

            await SqLiteDbContext.SaveChangesAsync();
        }

        public async Task<List<NoteTypeWebDto>> SelectAllNoteTypeWebDtoList()
        {
            var taskRes = await Task.Run(() =>
            {
                return SqLiteDbContext.NoteType.ToList();
            });

            return taskRes;
        }

        public async Task<List<NoteCategoryWebDto>> SelectAllNoteCategoryWebDtoList()
        {
            var taskRes = await Task.Run(() =>
            {
                return SqLiteDbContext.NoteCategory.ToList();
            });

            return taskRes;
        }

        public async Task<List<NoteContentWebDto>> SelectAllNoteContentWebDtoList()
        {
            var taskRes = await Task.Run(() =>
            {
                return SqLiteDbContext.NoteContent.ToList();
            });

            return taskRes;
        }

        public async Task ConvertImportFileContentToDbAsync(List<NoteTypeWebDto> noteTypeWebDtoList,
            List<NoteCategoryWebDto> noteCategoryWebDtoList, List<NoteContentWebDto> noteContentWebDtoList)
        {
            if (!noteTypeWebDtoList.IsNullOrEmpty())
            {
                foreach (var item in SqLiteDbContext.NoteType)
                {
                    SqLiteDbContext.NoteType.Remove(item);
                }

                await SqLiteDbContext.NoteType.AddRangeAsync(noteTypeWebDtoList);
            }

            if (!noteCategoryWebDtoList.IsNullOrEmpty())
            {
                foreach (var item in SqLiteDbContext.NoteCategory)
                {
                    SqLiteDbContext.NoteCategory.Remove(item);
                }

                await SqLiteDbContext.NoteCategory.AddRangeAsync(noteCategoryWebDtoList);
            }

            if (!noteContentWebDtoList.IsNullOrEmpty())
            {
                foreach (var item in SqLiteDbContext.NoteContent)
                {
                    SqLiteDbContext.NoteContent.Remove(item);
                }

                await SqLiteDbContext.NoteContent.AddRangeAsync(noteContentWebDtoList);
            }

            await SqLiteDbContext.SaveChangesAsync();
        }

        public async Task<List<NoteCategoryCustomWebDto>> SelectSearchMatchedListAsync(string typeName, string categoryName, string content)
        {
            var noteCategoryList = new List<NoteCategoryCustomWebDto>();

            var taskResult = await Task.Run(() =>
            {
                var sql = @"SELECT 
                             nt.UserId,
                             nt.TypeId,
                             nt.TypeName,
                             nc.CategoryId,
                             nc.CategoryName, 
                             nc.DisplayOrder AS CategoryDisplayOrder,
                             nc2.ContentId,
                             nt.Description AS TypeDescription
                            FROM NoteType nt
                            LEFT JOIN NoteCategory nc
                            ON nc.UserId = nt.UserId
                            AND nc.TypeId = nt.TypeId
                            LEFT JOIN NoteContent nc2
                            ON nc.UserId = nc2.UserId
                            AND nc.CategoryId = nc2.CategoryId
                            AND nc.TypeId = nc2.TypeId
                            WHERE nt.UserId = @UserId";

                if (!typeName.IsNullOrEmpty())
                {
                    sql += $" AND nt.TypeName LIKE '%{typeName}%'";
                }

                if (!categoryName.IsNullOrEmpty())
                {
                    sql += $" AND nc.CategoryName LIKE '%{categoryName}%'";
                }

                if (!content.IsNullOrEmpty())
                {
                    sql += $" AND nc2.Content LIKE '%{content}%'";
                }

                var noteCategoryModelList = SqLiteDbContext.Database.ExecuteRawSqlQueryAutoMapper<NoteCategoryCustomWebDto>(sql, dbCommand =>
                {
                    var userIdParam = dbCommand.CreateParameter();
                    userIdParam.ParameterName = "@UserId";
                    userIdParam.DbType = DbType.Int32;
                    userIdParam.Value = LoginInfo.UserBasicInfo.UserId;
                    dbCommand.Parameters.Add(userIdParam);
                });

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
