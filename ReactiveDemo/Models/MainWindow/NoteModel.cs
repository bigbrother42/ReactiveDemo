﻿using AutoMapper;
using BaseDemo.Util;
using BaseDemo.Util.Extensions;
using DataDemo.WebDto;
using ReactiveDemo.Models.Csv;
using ReactiveDemo.Models.UiModel;
using ServiceDemo.Common.SQLite;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ReactiveDemo.Models.MainWindow
{
    public class NoteModel
    {
        public static readonly string IMPORT_FILE_NAME_PREFIX_NOTE_TYPE = "note_type";
        public static readonly string IMPORT_FILE_NAME_PREFIX_NOTE_CATEGORY = "note_category";
        public static readonly string IMPORT_FILE_NAME_PREFIX_NOTE_CONTENT = "note_content";

        #region field name in import file(csv)

        public static readonly string FIELD_NAME_IN_IMPORT_FILE_USER_ID = "user_id";
        public static readonly string FIELD_NAME_IN_IMPORT_FILE_TYPE_ID = "type_id";
        public static readonly string FIELD_NAME_IN_IMPORT_FILE_TYPE_NAME = "type_name";
        public static readonly string FIELD_NAME_IN_IMPORT_FILE_DESCRIPTION = "description";
        public static readonly string FIELD_NAME_IN_IMPORT_FILE_CATEGORY_ID = "category_id";
        public static readonly string FIELD_NAME_IN_IMPORT_FILE_CATEGORY_NAME = "category_name";
        public static readonly string FIELD_NAME_IN_IMPORT_FILE_DISPLAY_ORDER = "display_order";
        public static readonly string FIELD_NAME_IN_IMPORT_FILE_CONTENT_ID = "content_id";
        public static readonly string FIELD_NAME_IN_IMPORT_FILE_CONTENT = "content";

        #endregion

        private NoteService _noteService = new NoteService();

        public async Task<NoteCategoryWebDto> InsertOrUpdateNoteCategory(NoteCategoryUiModel noteCategoryUiModel)
        {
            var webDto = new NoteCategoryWebDto
            {
                UserId = LoginInfo.UserBasicInfo.UserId,
                TypeId = noteCategoryUiModel.TypeId,
                DisplayOrder = noteCategoryUiModel.CategoryDisplayOrder,
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
                TypeId = noteCategoryUiModel.TypeId,
                CategoryId = noteCategoryUiModel.CategorySeq,
                CategoryName = noteCategoryUiModel.CategoryName,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                CreateBy = LoginInfo.UserBasicInfo.UserId,
                UpdateBy = LoginInfo.UserBasicInfo.UserId
            };

            return await _noteService.DeleteNoteCategoryAsync(webDto);
        }

        public async Task<List<NoteCategoryTypeUiModel>> SelectAllNoteCategory()
        {
            var noteCustomerWebDtoList = await _noteService.SelectAllNoteCategoryListAsync();

            var customTypeList = new List<NoteCategoryTypeUiModel>();
            if (!noteCustomerWebDtoList.IsNullOrEmpty())
            {
                var typeGroup = noteCustomerWebDtoList.GroupBy(o => new { o.TypeId, o.TypeName });

                foreach (var type in typeGroup)
                {
                    var typeUiModel = new NoteCategoryTypeUiModel
                    {
                        TypeId = type.Key.TypeId,
                        TypeName = type.Key.TypeName
                    };

                    var categoryList = type.AsParallel().OrderBy(o => o.CategoryDisplayOrder).ToList();
                    foreach (var category in categoryList)
                    {
                        if (category.CategoryId == 0) continue;
                        
                        var categoryUiModel = new NoteCategoryUiModel
                        {
                            TypeId = category.TypeId,
                            CategoryDisplayOrder = category.CategoryDisplayOrder,
                            CategorySeq = category.CategoryId,
                            CategoryName = category.CategoryName,
                            ContentId = category.ContentId,
                            Content = category.Content
                        };

                        typeUiModel.CategoryList.Add(categoryUiModel);
                    }

                    customTypeList.Add(typeUiModel);
                }
            }

            return customTypeList;
        }

        public async Task InsertOrUpdateNoteContent(NoteCategoryUiModel noteCategoryUiModel)
        {
            var wenDto = new NoteContentWebDto
            {
                UserId = LoginInfo.UserBasicInfo.UserId,
                CategoryId = noteCategoryUiModel.CategorySeq,
                TypeId = noteCategoryUiModel.TypeId,
                ContentId = noteCategoryUiModel.ContentId,
                Content = noteCategoryUiModel.Content,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                CreateBy = LoginInfo.UserBasicInfo.UserId,
                UpdateBy = LoginInfo.UserBasicInfo.UserId
            };

            await _noteService.InsertOrUpdateNoteContentAsync(wenDto);
        }

        public async Task<List<NoteCategoryTypeUiModel>> SelectAllNoteTypeList()
        {
            var resultList = new List<NoteCategoryTypeUiModel>();

            var webDtoList = await _noteService.SelectAllNoteTypeListAsync();
            if (!webDtoList.IsNullOrEmpty())
            {
                foreach (var webDto in webDtoList)
                {
                    resultList.Add(new NoteCategoryTypeUiModel
                    {
                        TypeId = webDto.TypeId,
                        TypeName = webDto.TypeName,
                        Description = webDto.Description,
                        CreateAt = $"{webDto.CreateAt:yyyy-MM-dd HH:mm:ss}",
                        CreateBy = webDto.CreateBy,
                        UpdateAt = $"{webDto.UpdateAt:yyyy-MM-dd HH:mm:ss}",
                        UpdateBy = webDto.UpdateBy
                    });
                }
            }

            return resultList;
        }

        public async Task<int> InsertOrUpdateNoteTypeList(List<NoteCategoryTypeUiModel> screenModelList)
        {
            if (screenModelList.IsNullOrEmpty()) return 0;

            var webDtoList = new List<NoteTypeWebDto>();
            foreach (var screenModel in screenModelList)
            {
                webDtoList.Add(new NoteTypeWebDto
                {
                    TypeId = screenModel.TypeId,
                    TypeName = screenModel.TypeName,
                    Description = screenModel.Description,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    CreateBy = LoginInfo.UserBasicInfo.UserId,
                    UpdateBy = LoginInfo.UserBasicInfo.UserId
                });
            }

            var updateList = await _noteService.InsertOrUpdateNoteTypeListAsync(webDtoList);

            return updateList.Count;
        }

        public async Task UpdateCategoryDisplayOrder(List<NoteCategoryUiModel> categoryUiModelList)
        {
            if (categoryUiModelList.IsNullOrEmpty()) return;

            var webDtoList = new List<NoteCategoryWebDto>();
            foreach (var categoryUiModel in categoryUiModelList)
            {
                webDtoList.Add(new NoteCategoryWebDto
                { 
                    TypeId = categoryUiModel.TypeId,
                    UserId = LoginInfo.UserBasicInfo.UserId,
                    CategoryId = categoryUiModel.CategorySeq,
                    DisplayOrder = categoryUiModel.CategoryDisplayOrder
                });
            }

            await _noteService.UpdateCategoryDisplayOrderAsync(webDtoList);
        }

        public async Task<List<NoteTypeCsvModel>> GetAllNoteTypeList()
        {
            return (await _noteService.SelectAllNoteTypeWebDtoList())
                .Select(o => Mapper.Map<NoteTypeCsvModel>(o))
                .ToList();
        }

        public async Task<List<NoteCategoryCsvModel>> GetAllNoteCategoryList()
        {
            return (await _noteService.SelectAllNoteCategoryWebDtoList())
                .Select(o => Mapper.Map<NoteCategoryCsvModel>(o))
                .ToList();
        }

        public async Task<List<NoteContentCsvModel>> GetAllNoteContentList()
        {
            return (await _noteService.SelectAllNoteContentWebDtoList())
                .Select(o => Mapper.Map<NoteContentCsvModel>(o))
                .ToList();
        }

        public bool ValidateImportFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            if (!fileName.EndsWith(FileUtil.FILE_EXTENSION)) return false;

            if (!fileName.StartsWith(IMPORT_FILE_NAME_PREFIX_NOTE_TYPE) 
                && !fileName.StartsWith(IMPORT_FILE_NAME_PREFIX_NOTE_CATEGORY) 
                && !fileName.StartsWith(IMPORT_FILE_NAME_PREFIX_NOTE_CONTENT)) return false;

            return true;
        }

        public async Task ConvertImportFileContentToDb(List<NoteTypeWebDto> noteTypeWebDtoList,
            List<NoteCategoryWebDto> noteCategoryWebDtoList, List<NoteContentWebDto> noteContentWebDtoList)
        {
            await _noteService.ConvertImportFileContentToDbAsync(noteTypeWebDtoList, noteCategoryWebDtoList, noteContentWebDtoList);
        }
    }
}
