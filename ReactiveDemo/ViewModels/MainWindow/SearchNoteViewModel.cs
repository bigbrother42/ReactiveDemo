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
    public class SearchNoteViewModel : ViewModelBase
    {
        #region Field

        private ObservableCollection<NoteSearchUiModel> _searchResultCollection = new ObservableCollection<NoteSearchUiModel>();
        public ObservableCollection<NoteSearchUiModel> SearchResultCollection
        {
            get => _searchResultCollection;
            set => SetProperty(ref _searchResultCollection, value);
        }

        #endregion

        #region PrivateProperty

        private NoteModel _noteModel = new NoteModel();

        #endregion

        #region ReactiveCommand

        public AsyncReactiveCommand SearchCommand { get; set; }
        public ReactiveCommand ExecuteCommand { get; set; }

        #endregion

        #region ReactiveProperty

        public ReactiveProperty<string> SearchTypeName { get; set; }
        public ReactiveProperty<string> SearchCategoryName { get; set; }
        public ReactiveProperty<string> SearcContent { get; set; }

        public ReactiveProperty<NoteSearchUiModel> SelectedSearchResult { get; set; }

        #endregion

        #region Request



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

            SearchTypeName = new ReactiveProperty<string>(string.Empty).AddTo(DisposablePool);
            SearchCategoryName = new ReactiveProperty<string>(string.Empty).AddTo(DisposablePool);
            SearcContent = new ReactiveProperty<string>(string.Empty).AddTo(DisposablePool);
            SelectedSearchResult = new ReactiveProperty<NoteSearchUiModel>().AddTo(DisposablePool);
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            SearchCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            SearchCommand.Subscribe(Search).AddTo(DisposablePool);

            ExecuteCommand = new ReactiveCommand().AddTo(DisposablePool);
            ExecuteCommand.Subscribe(Execute).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private async Task Search()
        {
            var resultList = await _noteModel.SelectSearchMatchedList(new NoteSearchCondition
            { 
                TypeName = SearchTypeName.Value ?? string.Empty,
                CategoryName = SearchCategoryName.Value ?? string.Empty,
                Content = SearcContent.Value ?? string.Empty
            });

            SearchResultCollection = new ObservableCollection<NoteSearchUiModel>(resultList.OrderBy(o => o.TypeId).ThenBy(o => o.CategoryId));
            var id = 1;
            foreach (var searchResult in SearchResultCollection)
            {
                searchResult.Id = id++;
            }

            if (SelectedSearchResult.Value == null)
            {
                SelectedSearchResult.Value = SearchResultCollection.FirstOrDefault();
            }
            else
            {
                SelectedSearchResult.Value = SearchResultCollection.FirstOrDefault(o => o.UserId == SelectedSearchResult.Value.UserId
                    && o.TypeId == SelectedSearchResult.Value.TypeId
                    && o.CategoryId == SelectedSearchResult.Value.CategoryId);
            }
        }

        private void Execute()
        {
            Notification.Content = SelectedSearchResult.Value;

            FinishInteraction?.Invoke();
        }

        #endregion
    }
}
