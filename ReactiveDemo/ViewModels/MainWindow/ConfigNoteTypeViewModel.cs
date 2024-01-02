using BaseDemo.Util.Extensions;
using GongSolutions.Wpf.DragDrop;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveDemo.Constants;
using ReactiveDemo.Models.MainWindow;
using ReactiveDemo.Models.UiModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReactiveDemo.ViewModels.MainWindow
{
    public class ConfigNoteTypeViewModel : ViewModelBase, IDropTarget
    {
        #region Field

        public ObservableCollection<NoteCategoryTypeUiModel> _originalNoteTypeCollection { get; set; } = new ObservableCollection<NoteCategoryTypeUiModel>();

        private ObservableCollection<NoteCategoryTypeUiModel> _noteTypeCollection = new ObservableCollection<NoteCategoryTypeUiModel>();
        public ObservableCollection<NoteCategoryTypeUiModel> NoteTypeCollection
        {
            get => _noteTypeCollection;
            set => SetProperty(ref _noteTypeCollection, value);
        }

        #endregion

        #region PrivateProperty

        private NoteModel _noteModel = new NoteModel();

        #endregion

        #region ReactiveCommand

        public AsyncReactiveCommand SaveCommand { get; set; }

        public ReactiveCommand<NoteCategoryTypeUiModel> DeleteCommand { get; set; }

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
                _originalNoteTypeCollection.AddRange(dbList);
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

            DeleteCommand = new ReactiveCommand<NoteCategoryTypeUiModel>().AddTo(DisposablePool);
            DeleteCommand.Subscribe(Delete).AddTo(DisposablePool);

            FinishInteractionCommand = new Prism.Commands.DelegateCommand(CloseWindow);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private void CloseWindow()
        {
            Notification.Content = NoteTypeCollection;

            FinishInteraction?.Invoke();
        }

        private async Task Save()
        {
            // insert or update
            await _noteModel.InsertOrUpdateNoteTypeList(NoteTypeCollection.ToList());

            // delete
            var needDeleteItemList = _originalNoteTypeCollection.Where(o => NoteTypeCollection.All(x => x.TypeId != o.TypeId)).ToList();
            await _noteModel.DeleteNoteTypeList(needDeleteItemList);

            CloseWindow();
        }

        private void Delete(NoteCategoryTypeUiModel deleteItem)
        {
            var result = MessageBox.Show(string.Format(MessageBoxConstant.DELETE_TYPE_CONFIRM_MESSAGE, deleteItem.TypeName), MessageBoxConstant.TITLE_WARNING, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                NoteTypeCollection.Remove(deleteItem);
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Move;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
        }

        public void Drop(IDropInfo dropInfo)
        {
            var dragItem = dropInfo.Data as NoteCategoryTypeUiModel;
            var targetItem = dropInfo.TargetItem as NoteCategoryTypeUiModel;

            if (dragItem != null && targetItem != null)
            {
                if (dragItem.TypeId == targetItem.TypeId) return;

                var sourceIndex = NoteTypeCollection.IndexOf(dragItem);
                var targetIndex = NoteTypeCollection.IndexOf(targetItem);

                NoteTypeCollection.Move(sourceIndex, targetIndex);

                var displayOrder = 1;
                foreach (var type in NoteTypeCollection)
                {
                    type.DisplayOrder = displayOrder++;
                }
            }
        }

        #endregion
    }
}
