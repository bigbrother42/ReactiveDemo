﻿using BaseDemo.Util.Extensions;
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
                SqLiteDbContext.NoteCategory.Add(param);
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

            var taskResult = await Task.Run(() =>
            {
                SqLiteDbContext.SaveChanges();

                return param;
            });

            return null;
        }

        public async Task<NoteContentWebDto> InsertOrUpdateNoteContentAsync(NoteContentWebDto param)
        {
            if (param == null) return null;

            var existContent = SqLiteDbContext.NoteContent.FirstOrDefault(o => o.TypeId == param.TypeId && o.CategoryId == param.CategoryId);
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

        public async Task<List<NoteTypeWebDto>> InsertOrUpdateNoteTypeListAsync(List<NoteTypeWebDto> paramList)
        {
            if (paramList.IsNullOrEmpty()) return new List<NoteTypeWebDto>();

            foreach (var param in paramList)
            {
                var existContent = SqLiteDbContext.NoteType.FirstOrDefault(o => o.TypeId == param.TypeId);
                if (existContent == null)
                {
                    // insert
                    SqLiteDbContext.NoteType.Add(param);
                }
                else
                {
                    // update
                    existContent.TypeName = param.TypeName;
                    existContent.Description = param.Description;
                }
            }

            await Task.Run(() =>
            {
                SqLiteDbContext.SaveChanges();

                return paramList;
            });

            return new List<NoteTypeWebDto>();
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

                var taskResult = await Task.Run(() =>
                {
                    return SqLiteDbContext.SaveChanges();
                });

                return taskResult;
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
    }
}
