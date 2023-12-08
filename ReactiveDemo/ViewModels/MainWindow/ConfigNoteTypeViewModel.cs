using BaseDemo.Util.Extensions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveDemo.Models.MainWindow;
using ReactiveDemo.Models.UiModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.ViewModels.MainWindow
{
    public class ConfigNoteTypeViewModel : ViewModelBase
    {
        #region Field

        public ObservableCollection<NoteCategoryTypeUiModel> NoteTypeCollection { get; set; } = new ObservableCollection<NoteCategoryTypeUiModel>();

        #endregion

        #region PrivateProperty

        private NoteModel _noteModel = new NoteModel();

        #endregion

        #region ReactiveCommand

        public AsyncReactiveCommand SaveCommand { get; set; }

        #endregion

        #region ReactiveProperty



        #endregion

        #region Request



        #endregion

        #region Events



        #endregion

        #region Override

        protected override async void InitData()
        {
            base.InitData();

            var dbList = await _noteModel.SelectAllNoteTypeList();
            if (!dbList.IsNullOrEmpty())
            {
                NoteTypeCollection.AddRange(dbList);
            }
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            SaveCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            SaveCommand.Subscribe(Save).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private void CloseWindow()
        {
            FinishInteraction?.Invoke();
        }

        private async Task Save()
        {
            await _noteModel.InsertOrUpdateNoteTypeList(NoteTypeCollection.ToList());

            CloseWindow();
        }

        #endregion
    }
}
