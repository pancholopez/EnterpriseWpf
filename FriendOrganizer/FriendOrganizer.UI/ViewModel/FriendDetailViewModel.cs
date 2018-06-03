using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendRepository _repository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;
        private FriendWrapper _friend;
        private bool _hasChanges;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }

        public FriendWrapper Friend
        {
            get => _friend;
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }
        public bool HasChanges
        {
            get => _hasChanges;
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public FriendDetailViewModel(IFriendRepository repository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
        {
            _repository = repository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _programmingLanguageLookupDataService = programmingLanguageLookupDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
        }

        private async void OnDeleteExecute()
        {
            var result = _messageDialogService
                .ShowOkCancelDialog($"Are you sure to delete user: {Friend.FirstName} {Friend.LastName}?",
                    "Question");

            if(result == MessageDialogResult.Cancel) return;
            _repository.Remove(Friend.Model);
            await _repository.SaveAsync();
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }

        private async void OnSaveExecute()
        {
            await _repository.SaveAsync();
            HasChanges = _repository.HasChanges();
            _eventAggregator.GetEvent<AfterFriendSaveEvent>().Publish(
                new AfterFriendSaveEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }

        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue
            ? await _repository.GetByIdAsync(friendId.Value)
                : CreateNewFriend();
            InitializeFriend(friend);
            await LoadProgrammingLanguagesLookupAsync();
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _repository.HasChanges();
                }
                if (e.PropertyName != nameof(Friend.HasErrors)) return;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id < 0)
            {
                //trigger validation for new friend
                Friend.FirstName = string.Empty;
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem{DisplayMember = " - "});
            var lookupItems = await _programmingLanguageLookupDataService
                .GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookupItems)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _repository.Add(friend);
            return friend;
        }
    }
}