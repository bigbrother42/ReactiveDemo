using BaseDemo.Util.Extensions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveDemo.Base.ActionBase;
using ReactiveDemo.Models.MainWindow;
using ReactiveDemo.Models.UiModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ReactiveDemo.ViewModels.MainWindow
{
    public class NoteViewModel : ViewModelBase
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

        #endregion

        #region Request

        public InteractionRequest<MethodNotification> AddNoteCategoryRequest { get; set; } = new InteractionRequest<MethodNotification>();

        public InteractionRequest<Notification> ConfigTypeViewRequest { get; set; } = new InteractionRequest<Notification>();

        #endregion

        #region Events



        #endregion

        #region Override

        protected override async void InitData()
        {
            base.InitData();

            var typeList = await _noteModel.SelectAllNoteTypeList();
            if (!typeList.IsNullOrEmpty())
            {
                NoteTypeCollection.AddRange(typeList);

                var categoryList = await _noteModel.SelectAllNoteCategory();
                if (!categoryList.IsNullOrEmpty())
                {
                    foreach (var type in NoteTypeCollection)
                    {
                        type.CategoryList.AddRange(categoryList.Where(o => o.TypeId == type.TypeId));
                    }

                    SelectedNoteType.Value = NoteTypeCollection.FirstOrDefault();
                    SelectedNoteCategory.Value = SelectedNoteType.Value.CategoryList.FirstOrDefault();
                }
            }
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();

            SelectedNoteCategory = new ReactiveProperty<NoteCategoryUiModel>().AddTo(DisposablePool);
            SelectedNoteType = new ReactiveProperty<NoteCategoryTypeUiModel>().AddTo(DisposablePool);

            SelectedNoteType.Skip(1).Subscribe(o =>
            {
                if (o == null) return;

                if (!o.CategoryList.IsNullOrEmpty())
                {
                    SelectedNoteCategory.Value = o.CategoryList.FirstOrDefault();
                }
            }).AddTo(DisposablePool);
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

            var newCategory = new NoteCategoryUiModel
            {
                TypeId = SelectedNoteType.Value.TypeId,
                //CategorySeq = newCategorySeq,
                CategoryName = "New Category"
            };

            SelectedNoteType.Value.CategoryList.Add(newCategory);
            SelectedNoteCategory.Value = newCategory;

            if (SelectedNoteCategory.Value != null)
            {
                SelectedNoteCategory.Value.IsEdit = true;
            }

            AddNoteCategoryRequest.Raise(new MethodNotification());
        }

        private async Task DeleteNoteCategoryAsync(NoteCategoryUiModel deleteItem)
        {
            if (deleteItem == null) return;

            var deleteNum = await _noteModel.DeleteNoteCategory(deleteItem);
            if (deleteNum > 0)
            {
                SelectedNoteType.Value.CategoryList.ExRemoveAll(o => o.TypeId == deleteItem.TypeId && o.CategorySeq == deleteItem.CategorySeq);
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

        }

        private void ConfigType()
        {
            ConfigTypeViewRequest.Raise(new Notification(), o => {
                
            });
        }

        #endregion
    }
}
