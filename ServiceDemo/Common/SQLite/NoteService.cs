using BaseDemo.Util.Extensions;
using DataDemo.WebDto;
using ServiceDemo.Base;
using SharedDemo.GlobalData;
using SharedDemo.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDemo.Common.SQLite
{
    public class NoteService : ClientApiService
    {
        public async Task<int> InsertNoteCategory(NoteCategoryWebDto param)
        {
            if (param == null) return 0;

            SqLiteDbContext.NoteCategory.Add(param);

            var taskResult = await Task.Run(() =>
            {
                return SqLiteDbContext.SaveChanges();
            });

            return taskResult;
        }

        public async Task<List<NoteCategoryWebDto>> SelectAllNoteCategoryListAsync()
        {
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
                            FROM NoteCategory nc";

                var noteCategoryModelList = SqLiteDbContext.Database.ExecuteRawSqlQueryAutoMapper<NoteCategoryWebDto>(sql);

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
