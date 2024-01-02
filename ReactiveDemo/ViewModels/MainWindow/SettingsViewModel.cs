using BaseDemo.Util;
using DataDemo.WebDto;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveDemo.Constants;
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
using System.Windows;
using System.Windows.Forms;

namespace ReactiveDemo.ViewModels.MainWindow
{
    public class SettingsViewModel : ViewModelBase
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

        public AsyncReactiveCommand ResetCommand { get; set; }

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

            ResetCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            ResetCommand.Subscribe(Reset).AddTo(DisposablePool);
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

        private async Task Reset()
        {
            var res = System.Windows.MessageBox.Show(MessageBoxConstant.RESET_NOTE_APPLICATION_CONFIRM_MESSAGE, MessageBoxConstant.TITLE_WARNING, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                await _noteModel.ResetNoteApplication();

                // need to restart application
                RestartApplication();
            }
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

                        _noteModel.ExportToLocal(fbd.SelectedPath);
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
                var res = System.Windows.MessageBox.Show(MessageBoxConstant.IMPORT_FILE_CONFIRM_MESSAGE, MessageBoxConstant.TITLE_WARNING, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    using (var ofd = new OpenFileDialog())
                    {
                        var result = ofd.ShowDialog();

                        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                        {
                            IsProgress.Value = true;

                            //await _noteModel.ImportLocalZip(ofd.FileName);

                            // need to restart application
                            RestartApplication();
                        }
                    }
                }
            }
            finally
            {
                IsProgress.Value = false;
            }
        }

        public async Task ImportForDrop(string zipFilePath)
        {
            try
            {
                IsProgress.Value = true;

                //await _noteModel.ImportLocalZip(zipFilePath);

                RestartApplication();
            }
            finally
            {
                IsProgress.Value = false;
            }
        }

        private void RestartApplication()
        {
            System.Windows.Application.Current.Shutdown();
            var processModule = Process.GetCurrentProcess().MainModule;
            if (processModule != null)
            {
                Process.Start(processModule.FileName, "ReStart");
            }
        }

        #endregion
    }
}
