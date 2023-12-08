using Reactive.Bindings;
using Reactive.Bindings.Extensions;
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



        #endregion

        #region ReactiveCommand

        public ReactiveCommand SaveCommand { get; set; }

        #endregion

        #region ReactiveProperty



        #endregion

        #region Request



        #endregion

        #region Events



        #endregion

        #region Override

        protected override void InitData()
        {
            base.InitData();

            NoteTypeCollection.Add(new NoteCategoryTypeUiModel
            {
                TypeId = 1,
                TypeName = "test1"
            });

            NoteTypeCollection.Add(new NoteCategoryTypeUiModel
            {
                TypeId = 2,
                TypeName = "test2"
            });
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            SaveCommand = new ReactiveCommand().AddTo(DisposablePool);
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

        private void Save()
        {
            CloseWindow();
        }

        #endregion
    }
}
