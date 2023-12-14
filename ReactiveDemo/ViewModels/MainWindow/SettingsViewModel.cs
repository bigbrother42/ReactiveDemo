using BaseDemo.Util;
using DataDemo.WebDto;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveDemo.Models.Csv;
using ReactiveDemo.Models.MainWindow;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReactiveDemo.ViewModels.MainWindow
{
    public class SettingsViewModel : ViewModelBase//, GongSolutions.Wpf.DragDrop.IDropTarget
    {

        #region Field

        

        #endregion

        #region PrivateProperty

        private NoteModel _noteModel = new NoteModel();

        #endregion

        #region ReactiveCommand

        public ReactiveCommand AccountViewCommand { get; set; }

        public AsyncReactiveCommand ExportCommand { get; set; }

        public AsyncReactiveCommand ImportCommand { get; set; }

        #endregion

        #region ReactiveProperty

        public ReactiveProperty<bool> IsProgress { get; set; }

        #endregion

        #region Request

        public InteractionRequest<Notification> AccountViewRequest { get; set; } = new InteractionRequest<Notification>();

        #endregion

        #region Events



        #endregion

        #region Override

        protected override void InitData()
        {
            base.InitData();
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();

            IsProgress = new ReactiveProperty<bool>().AddTo(DisposablePool);
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            AccountViewCommand = new ReactiveCommand().AddTo(DisposablePool);
            AccountViewCommand.Subscribe(AccountView).AddTo(DisposablePool);

            ExportCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            ExportCommand.Subscribe(Export).AddTo(DisposablePool);

            ImportCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            ImportCommand.Subscribe(Import).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private void AccountView()
        {
            AccountViewRequest.Raise(new Notification(), notification => { });
        }

        private async Task Export()
        {
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    var result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        IsProgress.Value = true;

                        // note type
                        var noteTypeList = await _noteModel.GetAllNoteTypeList();
                        var noteTypeFile = CsvFileUtil<NoteTypeCsvModel>.CsvFileGenerate(noteTypeList, fbd.SelectedPath, "note_type");

                        // note category
                        var noteCategoryList = await _noteModel.GetAllNoteCategoryList();
                        var noteCategoryFile = CsvFileUtil<NoteCategoryCsvModel>.CsvFileGenerate(noteCategoryList, fbd.SelectedPath, "note_category");

                        // note content
                        var noteContentList = await _noteModel.GetAllNoteContentList();
                        var noteContentFile = CsvFileUtil<NoteContentCsvModel>.CsvFileGenerate(noteContentList, fbd.SelectedPath, "note_content");

                        // zip
                        var zipPath = $@"{fbd.SelectedPath}\note_{DateTime.Now:yyyyMMddHHmmss}.zip";
                        var filePathList = new List<string> { noteTypeFile, noteCategoryFile, noteContentFile };
                        ZipUtil.ZipFile(filePathList, zipPath);
                    }
                }
            }
            finally
            {
                IsProgress.Value = false;
            }
        }

        private async Task Import()
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    var result = ofd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                    {
                        IsProgress.Value = true;

                        ZipUtil.UnZipFile(ofd.FileName, GlobalData.CSV_PATH, out string errorMessage);

                        var fileList = Directory.GetFiles(GlobalData.CSV_PATH);
                        var noteTypeWebDtoList = new List<NoteTypeWebDto>();
                        var noteCategoryWebDtoList = new List<NoteCategoryWebDto>();
                        var noteContentWebDtoList = new List<NoteContentWebDto>();
                        var operateTime = DateTime.Now;
                        foreach (var file in fileList)
                        {
                            if (!_noteModel.ValidateImportFileName(file)) continue;

                            var dataTable = FileUtil.ConvertCsvToDataTable(file);
                            var fileName = Path.GetFileName(file);

                            if (fileName.StartsWith(NoteModel.IMPORT_FILE_NAME_PREFIX_NOTE_TYPE))
                            {
                                // note type
                                for (var i = 0; i < dataTable.Rows.Count; i++)
                                {
                                    var userIdStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_USER_ID].ToString();
                                    var typeIdStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_TYPE_ID].ToString();
                                    var typeNameStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_TYPE_NAME].ToString();
                                    var descriptionStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_DESCRIPTION].ToString();

                                    noteTypeWebDtoList.Add(new NoteTypeWebDto
                                    {
                                        UserId = int.Parse(userIdStr),
                                        TypeId = int.Parse(typeIdStr),
                                        TypeName = typeNameStr,
                                        Description = descriptionStr,
                                        CreateAt = operateTime,
                                        CreateBy = LoginInfo.UserBasicInfo.UserId,
                                        UpdateAt = operateTime,
                                        UpdateBy = LoginInfo.UserBasicInfo.UserId,
                                    });
                                }
                            }
                            else if (fileName.StartsWith(NoteModel.IMPORT_FILE_NAME_PREFIX_NOTE_CATEGORY))
                            {
                                // note category
                                for (var i = 0; i < dataTable.Rows.Count; i++)
                                {
                                    var userIdStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_USER_ID].ToString();
                                    var typeIdStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_TYPE_ID].ToString();
                                    var displayOrderStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_DISPLAY_ORDER].ToString();
                                    var categoryIdStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_CATEGORY_ID].ToString();
                                    var categoryNameStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_CATEGORY_NAME].ToString();

                                    noteCategoryWebDtoList.Add(new NoteCategoryWebDto
                                    {
                                        UserId = int.Parse(userIdStr),
                                        TypeId = int.Parse(typeIdStr),
                                        DisplayOrder = int.Parse(displayOrderStr),
                                        CategoryId = int.Parse(categoryIdStr),
                                        CategoryName = categoryNameStr,
                                        CreateAt = operateTime,
                                        CreateBy = LoginInfo.UserBasicInfo.UserId,
                                        UpdateAt = operateTime,
                                        UpdateBy = LoginInfo.UserBasicInfo.UserId,
                                    });
                                }
                            }
                            else if (fileName.StartsWith(NoteModel.IMPORT_FILE_NAME_PREFIX_NOTE_CONTENT))
                            {
                                // note content
                                for (var i = 0; i < dataTable.Rows.Count; i++)
                                {
                                    var userIdStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_USER_ID].ToString();
                                    var typeIdStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_TYPE_ID].ToString();
                                    var contentIdStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_CONTENT_ID].ToString();
                                    var categoryIdStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_CATEGORY_ID].ToString();
                                    var contentStr = dataTable.Rows[i][NoteModel.FIELD_NAME_IN_IMPORT_FILE_CONTENT].ToString();

                                    noteContentWebDtoList.Add(new NoteContentWebDto
                                    {
                                        UserId = int.Parse(userIdStr),
                                        TypeId = int.Parse(typeIdStr),
                                        CategoryId = int.Parse(categoryIdStr),
                                        ContentId = int.Parse(contentIdStr),
                                        Content = contentStr,
                                        CreateAt = operateTime,
                                        CreateBy = LoginInfo.UserBasicInfo.UserId,
                                        UpdateAt = operateTime,
                                        UpdateBy = LoginInfo.UserBasicInfo.UserId,
                                    });
                                }
                            }
                        }

                        await _noteModel.ConvertImportFileContentToDb(noteTypeWebDtoList, noteCategoryWebDtoList, noteContentWebDtoList);

                        // need to restart application
                        System.Windows.Application.Current.Shutdown();
                        var processModule = Process.GetCurrentProcess().MainModule;
                        if (processModule != null)
                        {
                            Process.Start(processModule.FileName, "ReStart");
                        }
                    }
                }
            }
            finally
            {
                IsProgress.Value = false;
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = System.Windows.DragDropEffects.Link;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
        }

        public void Drop(IDropInfo dropInfo)
        {
            // do nothing
        }

        #endregion
    }
}
