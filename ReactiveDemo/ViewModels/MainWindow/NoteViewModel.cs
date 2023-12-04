﻿using BaseDemo.Util.Extensions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveDemo.Base.ActionBase;
using ReactiveDemo.Models.UiModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.ViewModels.MainWindow
{
    public class NoteViewModel : ViewModelBase
    {
        #region Field

        public ObservableCollection<NoteCategoryUiModel> NoteCategoryCollection { get; set; } = new ObservableCollection<NoteCategoryUiModel>();

        #endregion

        #region PrivateProperty



        #endregion

        #region ReactiveCommand

        public AsyncReactiveCommand AddNoteCategoryCommand { get; set; }

        public ReactiveCommand<NoteCategoryUiModel> DeleteNoteCategoryCommand { get; set; }

        public ReactiveCommand<NoteCategoryUiModel> EditNoteCategoryNameCommand { get; set; }

        #endregion

        #region ReactiveProperty

        public ReactiveProperty<NoteCategoryUiModel> SelectedNoteCategory { get; set; }

        #endregion

        #region Request

        public InteractionRequest<MethodNotification> AddNoteCategoryRequest { get; set; } = new InteractionRequest<MethodNotification>();

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

            SelectedNoteCategory = new ReactiveProperty<NoteCategoryUiModel>().AddTo(DisposablePool);
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            AddNoteCategoryCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            AddNoteCategoryCommand.Subscribe(AddNoteCategory).AddTo(DisposablePool);

            DeleteNoteCategoryCommand = new ReactiveCommand<NoteCategoryUiModel>().AddTo(DisposablePool);
            DeleteNoteCategoryCommand.Subscribe(DeleteNoteCategory).AddTo(DisposablePool);

            EditNoteCategoryNameCommand = new ReactiveCommand<NoteCategoryUiModel>().AddTo(DisposablePool);
            EditNoteCategoryNameCommand.Subscribe(EditNoteCategoryName).AddTo(DisposablePool);
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

            var newCategorySeq = NoteCategoryCollection.Count() == 0 ? 1 : NoteCategoryCollection.Max(o => o.CategorySeq) + 1;

            var newCategory = new NoteCategoryUiModel
            {
                CategorySeq = newCategorySeq,
                CategoryName = "New Category"
            };

            NoteCategoryCollection.Add(newCategory);
            SelectedNoteCategory.Value = newCategory;

            if (SelectedNoteCategory.Value != null)
            {
                SelectedNoteCategory.Value.IsEdit = true;
            }

            AddNoteCategoryRequest.Raise(new MethodNotification());
        }

        private void DeleteNoteCategory(NoteCategoryUiModel deleteItem)
        {
            if (deleteItem == null) return;

            NoteCategoryCollection.ExRemoveAll(o => o.CategorySeq == deleteItem.CategorySeq);
        }

        private void EditNoteCategoryName(NoteCategoryUiModel editItem)
        {
            if (editItem == null) return;

            editItem.IsEdit = true;
        }

        #endregion
    }
}
