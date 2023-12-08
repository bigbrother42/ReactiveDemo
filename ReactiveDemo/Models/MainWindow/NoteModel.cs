﻿using BaseDemo.Util.Extensions;
using DataDemo.WebDto;
using ReactiveDemo.Models.UiModel;
using ServiceDemo.Common.SQLite;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ReactiveDemo.Models.MainWindow
{
    public class NoteModel
    {
        private NoteService _noteService = new NoteService();

        public async Task<NoteCategoryWebDto> InsertOrUpdateNoteCategory(NoteCategoryUiModel noteCategoryUiModel)
        {
            var webDto = new NoteCategoryWebDto
            {
                UserId = LoginInfo.UserBasicInfo.UserId,
                CategoryId = noteCategoryUiModel.CategorySeq,
                CategoryName = noteCategoryUiModel.CategoryName,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                CreateBy = LoginInfo.UserBasicInfo.UserId,
                UpdateBy = LoginInfo.UserBasicInfo.UserId
            };

            return await _noteService.InsertOrUpdateNoteCategoryAsync(webDto);
        }

        public async Task<int> DeleteNoteCategory(NoteCategoryUiModel noteCategoryUiModel)
        {
            var webDto = new NoteCategoryWebDto
            {
                UserId = LoginInfo.UserBasicInfo.UserId,
                CategoryId = noteCategoryUiModel.CategorySeq,
                CategoryName = noteCategoryUiModel.CategoryName,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                CreateBy = LoginInfo.UserBasicInfo.UserId,
                UpdateBy = LoginInfo.UserBasicInfo.UserId
            };

            return await _noteService.DeleteNoteCategoryAsync(webDto);
        }

        public async Task<List<NoteCategoryUiModel>> SelectAllNoteCategory()
        {
            var noteCategoryWebDtoList = await _noteService.SelectAllNoteCategoryListAsync();

            var customCategoryList = new List<NoteCategoryUiModel>();
            if (!noteCategoryWebDtoList.IsNullOrEmpty())
            {
                foreach (var noteCategoryWebDto in noteCategoryWebDtoList)
                {
                    var uiModle = new NoteCategoryUiModel
                    {
                        CategorySeq = noteCategoryWebDto.CategoryId,
                        CategoryName = noteCategoryWebDto.CategoryName
                    };

                    if (!noteCategoryWebDto.Content.IsNullOrEmpty())
                    {
                        uiModle.Content = noteCategoryWebDto.Content;
                    }

                    customCategoryList.Add(uiModle);
                }
            }

            return customCategoryList;
        }

        public async Task InsertOrUpdateNoteContent(NoteCategoryUiModel noteCategoryUiModel)
        {
            var wenDto = new NoteContentWebDto
            {
                UserId = LoginInfo.UserBasicInfo.UserId,
                CategoryId = noteCategoryUiModel.CategorySeq,
                Content = noteCategoryUiModel.Content,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                CreateBy = LoginInfo.UserBasicInfo.UserId,
                UpdateBy = LoginInfo.UserBasicInfo.UserId
            };

            await _noteService.InsertOrUpdateNoteContentAsync(wenDto);
        }
    }
}