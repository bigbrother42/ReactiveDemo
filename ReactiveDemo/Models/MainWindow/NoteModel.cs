using DataDemo.WebDto;
using ReactiveDemo.Models.UiModel;
using ServiceDemo.Common.SQLite;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Models.MainWindow
{
    public class NoteModel
    {
        private NoteService _noteService = new NoteService();

        public async Task<int> InsertNoteCategory(NoteCategoryUiModel noteCategoryUiModel)
        {
            var webDto = new NoteCategoryWebDto
            {
                CategoryName = noteCategoryUiModel.CategoryName,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                CreateBy = LoginInfo.UserBasicInfo.UserId,
                UpdateBy = LoginInfo.UserBasicInfo.UserId
            };

            var insertNum = await _noteService.InsertNoteCategory(webDto);

            return insertNum;
        }

        public async Task<List<NoteCategoryUiModel>> SelectAllNoteCategory()
        {
            var noteCategoryWebDtoList = await _noteService.SelectAllNoteCategoryListAsync();

            return noteCategoryWebDtoList.Select(o => new NoteCategoryUiModel {
                CategorySeq = o.CategoryId,
                CategoryName = o.CategoryName
            }).ToList();
        }
    }
}
