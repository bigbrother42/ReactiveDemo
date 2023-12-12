using BaseDemo.Util;
using DataDemo.WebDto;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveDemo.Models.Csv;
using ReactiveDemo.Models.MainWindow;
using SharedDemo.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ReactiveDemo.ViewModels.MainWindow
{
    public class SettingsViewModel : ViewModelBase, GongSolutions.Wpf.DragDrop.IDropTarget
    {

        #region Field



        #endregion

        #region PrivateProperty

        private NoteModel _noteModel = new NoteModel();

        #endregion

        #region ReactiveCommand

        public ReactiveCommand AccountViewCommand { get; set; }

        public AsyncReactiveCommand ExportCommand { get; set; }

        public ReactiveCommand ImportCommand { get; set; }

        #endregion

        #region ReactiveProperty



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
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            AccountViewCommand = new ReactiveCommand().AddTo(DisposablePool);
            AccountViewCommand.Subscribe(AccountView).AddTo(DisposablePool);

            ExportCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            ExportCommand.Subscribe(Export).AddTo(DisposablePool);

            ImportCommand = new ReactiveCommand().AddTo(DisposablePool);
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
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    // note type
                    var noteTypeList = await _noteModel.GetAllNoteTypeWebDtoList();
                    CsvFileUtil<NoteTypeCsvModel>.CsvFileGenerate(noteTypeList, fbd.SelectedPath, "note_type");

                    // note category


                    // note content

                }
            }
        }

        private void Import()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var importDbPath = Path.ChangeExtension(fbd.SelectedPath, ".sqlite3");

                }
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = System.Windows.DragDropEffects.Link;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
        }

        public void Drop(IDropInfo dropInfo)
        {
            
        }

        #endregion
    }
}
