using BaseDemo.Helpers.Component;
using BaseDemo.Util.Extensions;
using GongSolutions.Wpf.DragDrop;
using Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveDemo.Base.ActionBase;
using ReactiveDemo.Constants;
using ReactiveDemo.Models.MainWindow;
using ReactiveDemo.Models.UiModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace ReactiveDemo.ViewModels.MainWindow
{
    public class NoteViewModel : ViewModelBase, IDropTarget
    {
        #region Field

        public ObservableCollection<NoteCategoryTypeUiModel> NoteTypeCollection { get; set; } = new ObservableCollection<NoteCategoryTypeUiModel>();

        #endregion

        #region PrivateProperty

        private NoteModel _noteModel = new NoteModel();

        #endregion

        #region ReactiveCommand

        public AsyncReactiveCommand AddNoteCategoryCommand { get; set; }

        public AsyncReactiveCommand<NoteCategoryUiModel> DeleteNoteCategoryCommand { get; set; }

        public ReactiveCommand<NoteCategoryUiModel> EditNoteCategoryNameCommand { get; set; }

        public AsyncReactiveCommand SaveContentCommand { get; set; }

        public ReactiveCommand SearchContentCommand { get; set; }

        public ReactiveCommand ConfigTypeCommand { get; set; }

        #endregion

        #region ReactiveProperty

        public ReactiveProperty<NoteCategoryUiModel> SelectedNoteCategory { get; set; }

        public ReactiveProperty<NoteCategoryTypeUiModel> SelectedNoteType { get; set; }

        public ReactiveProperty<bool> IsTextBoxEnable { get; set; }

        public ReactiveProperty<bool> IsBoldChecked { get; set; }

        public ReactiveProperty<bool> IsProgress { get; set; }

        #endregion

        #region Request

        public InteractionRequest<MethodNotification> AddNoteCategoryRequest { get; set; } = new InteractionRequest<MethodNotification>();

        public InteractionRequest<Notification> ConfigTypeViewRequest { get; set; } = new InteractionRequest<Notification>();

        public InteractionRequest<Notification> OpenSearchNoteViewRequest { get; set; } = new InteractionRequest<Notification>();

        #endregion

        #region Events



        #endregion

        #region Override

        protected override async void InitData()
        {
            base.InitData();

            var typeUiModelList = await _noteModel.SelectAllNoteCategory();
            if (!typeUiModelList.IsNullOrEmpty())
            {
                NoteTypeCollection.AddRange(typeUiModelList);

                SelectedNoteType.Value = NoteTypeCollection.FirstOrDefault();
                SelectedNoteCategory.Value = SelectedNoteType.Value.CategoryList.FirstOrDefault();
            }
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();

            SelectedNoteCategory = new ReactiveProperty<NoteCategoryUiModel>().AddTo(DisposablePool);
            SelectedNoteType = new ReactiveProperty<NoteCategoryTypeUiModel>().AddTo(DisposablePool);

            IsTextBoxEnable = new[] {
                SelectedNoteCategory.ObserveProperty(o => o.Value).Select(o => o != null),
            }.CombineLatest(x => x.Any(y => y)).ToReactiveProperty().AddTo(DisposablePool);

            SelectedNoteType.Skip(1).Subscribe(o =>
            {
                if (o == null) return;

                if (!o.CategoryList.IsNullOrEmpty())
                {
                    SelectedNoteCategory.Value = o.CategoryList.FirstOrDefault();
                }
            }).AddTo(DisposablePool);

            IsBoldChecked = new ReactiveProperty<bool>().AddTo(DisposablePool);
            IsProgress = new ReactiveProperty<bool>().AddTo(DisposablePool);
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            AddNoteCategoryCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            AddNoteCategoryCommand.Subscribe(AddNoteCategory).AddTo(DisposablePool);

            DeleteNoteCategoryCommand = new AsyncReactiveCommand<NoteCategoryUiModel>().AddTo(DisposablePool);
            DeleteNoteCategoryCommand.Subscribe(DeleteNoteCategoryAsync).AddTo(DisposablePool);

            EditNoteCategoryNameCommand = new ReactiveCommand<NoteCategoryUiModel>().AddTo(DisposablePool);
            EditNoteCategoryNameCommand.Subscribe(EditNoteCategoryName).AddTo(DisposablePool);

            SaveContentCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            SaveContentCommand.Subscribe(SaveContent).AddTo(DisposablePool);

            SearchContentCommand = new ReactiveCommand().AddTo(DisposablePool);
            SearchContentCommand.Subscribe(SearchContent).AddTo(DisposablePool);

            ConfigTypeCommand = new ReactiveCommand().AddTo(DisposablePool);
            ConfigTypeCommand.Subscribe(ConfigType).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private async Task AddNoteCategory()
        {
            if (SelectedNoteCategory.Value != null)
            {
                SelectedNoteCategory.Value.IsEdit = false;
            }

            var newCategorySeq = SelectedNoteType.Value.CategoryList.Count() == 0 ? 1 : SelectedNoteType.Value.CategoryList.Max(o => o.CategorySeq) + 1;
            var displayOrder = SelectedNoteType.Value.CategoryList.Count() == 0 ? 1 : SelectedNoteType.Value.CategoryList.Max(o => o.CategoryDisplayOrder) + 1;

            var newCategory = new NoteCategoryUiModel
            {
                TypeId = SelectedNoteType.Value.TypeId,
                CategoryDisplayOrder = displayOrder,
                CategorySeq = newCategorySeq,
                //ContentId = newCategorySeq,
                CategoryName = NoteModel.NEW_CATEGORY_NAME
            };

            SelectedNoteType.Value.CategoryList.Add(newCategory);
            SelectedNoteCategory.Value = newCategory;

            AddNoteCategoryRequest.Raise(new MethodNotification());
        }

        private async Task DeleteNoteCategoryAsync(NoteCategoryUiModel deleteItem)
        {
            if (deleteItem == null) return;

            var result = MessageBox.Show(string.Format(MessageBoxConstant.DELETE_CATEGORY_CONFIRM_MESSAGE, deleteItem.CategoryName), MessageBoxConstant.TITLE_WARNING, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var deleteNum = await _noteModel.DeleteNoteCategory(deleteItem);
                if (deleteNum > 0)
                {
                    SelectedNoteType.Value.CategoryList.ExRemoveAll(o => o.TypeId == deleteItem.TypeId && o.CategorySeq == deleteItem.CategorySeq);
                    SelectedNoteCategory.Value = SelectedNoteType.Value.CategoryList.FirstOrDefault();
                }
            }
        }

        private void EditNoteCategoryName(NoteCategoryUiModel editItem)
        {
            if (editItem == null) return;

            editItem.IsEdit = true;
        }

        private async Task SaveContent()
        {
            if (SelectedNoteCategory.Value == null) return;

            await _noteModel.InsertOrUpdateNoteContent(SelectedNoteCategory.Value);
        }

        private void SearchContent()
        {
            IsProgress.Value = true;

            try
            {
                OpenSearchNoteViewRequest.Raise(new Notification(), notification =>
                {
                    if (notification.Content is NoteSearchUiModel noteSearchUiModel)
                    {
                        SelectedNoteType.Value = NoteTypeCollection.FirstOrDefault(o => o.TypeId == noteSearchUiModel.TypeId);
                        SelectedNoteCategory.Value = SelectedNoteType.Value.CategoryList.FirstOrDefault(o => o.CategorySeq == noteSearchUiModel.CategoryId);
                    }
                });
            }
            finally
            {
                IsProgress.Value = false;
            }
        }

        private void ConfigType()
        {
            ConfigTypeViewRequest.Raise(new Notification(), notification => {
                if (notification.Content is ObservableCollection<NoteCategoryTypeUiModel> typeUiModelList)
                {
                    var needAddTypeList = typeUiModelList.Where(o => NoteTypeCollection.All(x => o.TypeId != x.TypeId)).ToList();
                    if (!needAddTypeList.IsNullOrEmpty())
                    {
                        NoteTypeCollection.AddRange(needAddTypeList);
                    }

                    var needUpdateTypeList = typeUiModelList.Where(o => NoteTypeCollection.Any(x => o.TypeId == x.TypeId && !string.Equals(o.TypeName, x.TypeName))).ToList();
                    if (!needUpdateTypeList.IsNullOrEmpty())
                    {
                        foreach (var needUpdateType in needUpdateTypeList)
                        {
                            var screenItem = NoteTypeCollection.FirstOrDefault(o => o.TypeId == needUpdateType.TypeId);
                            if (screenItem != null)
                            {
                                screenItem.TypeName = needUpdateType.TypeName;
                            }
                        }
                    }

                    var needADeleteTypeList = NoteTypeCollection.Where(o => typeUiModelList.All(x => o.TypeId != x.TypeId)).ToList();
                    if (!needADeleteTypeList.IsNullOrEmpty())
                    {
                        NoteTypeCollection.ExRemoveAll(o => needADeleteTypeList.Any(x => x.TypeId == o.TypeId));
                    }
                }
            });
        }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Move;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
        }

        public void Drop(IDropInfo dropInfo)
        {
            var dragItem = dropInfo.Data as NoteCategoryUiModel;
            var targetItem = dropInfo.TargetItem as NoteCategoryUiModel;

            if (dragItem != null && targetItem != null)
            {
                if (dragItem.CategorySeq == targetItem.CategorySeq) return;

                var sourceIndex = SelectedNoteType.Value.CategoryList.IndexOf(dragItem);
                var targetIndex = SelectedNoteType.Value.CategoryList.IndexOf(targetItem);

                SelectedNoteType.Value.CategoryList.Move(sourceIndex, targetIndex);

                SelectedNoteCategory.Value = dragItem;

                var displayOrder = 1;
                foreach (var category in SelectedNoteType.Value.CategoryList)
                {
                    category.CategoryDisplayOrder = displayOrder++;
                }

                _noteModel.UpdateCategoryDisplayOrder(SelectedNoteType.Value.CategoryList.ToList());
            }
        }

        #endregion
    }
}
