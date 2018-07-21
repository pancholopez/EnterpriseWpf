using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private MeetingWrapper _meeting;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMeetingRepository _meetingRepository;
        private Friend _selectedAvailableFriend;
        private Friend _selectedAddedFriend;
        private List<Friend> _allFriends;

        public MeetingDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository) : base(eventAggregator)
        {
            _messageDialogService = messageDialogService;
            _meetingRepository = meetingRepository;

            AddedFriends = new ObservableCollection<Friend>();
            AvailableFriends = new ObservableCollection<Friend>();
            AddFriendCommand = new DelegateCommand(OnAddFriendExecute, OnAddFriendCanExecute);
            RemoveFriendCommand = new DelegateCommand(OnRemoveFriendExecute, OnRemoveFriendCanExecute);
        }

        public ObservableCollection<Friend> AvailableFriends { get; }
        public ObservableCollection<Friend> AddedFriends { get; }

        public ICommand AddFriendCommand { get; }
        public ICommand RemoveFriendCommand { get; }

        private bool OnRemoveFriendCanExecute() => SelectedAddedFriend != null;

        private void OnRemoveFriendExecute()
        {
            var friendToRemove = SelectedAddedFriend;

            //todo:fix the navigation
            var friendMeeting = friendToRemove.FriendMeetings
                .Single(m => m.FriendId == friendToRemove.Id 
                             && m.MeetingId == Meeting.Model.Id);
            Meeting.Model.FriendMeetings.Remove(friendMeeting);

            AddedFriends.Remove(friendToRemove);
            AvailableFriends.Add(friendToRemove);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddFriendCanExecute() => SelectedAvailableFriend != null;

        private void OnAddFriendExecute()
        {
            var friendToAdd = SelectedAvailableFriend;

            //todo:fix the navigation
            Meeting.Model.FriendMeetings.Add(new FriendMeetings
            {
                Friend = friendToAdd,
                FriendId = friendToAdd.Id,
                Meeting = Meeting.Model,
                MeetingId = Meeting.Model.Id
            });

            AddedFriends.Add(friendToAdd);
            AvailableFriends.Remove(friendToAdd);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public Friend SelectedAvailableFriend
        {
            get => _selectedAvailableFriend;
            set
            {
                _selectedAvailableFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)AddFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public Friend SelectedAddedFriend
        {
            get => _selectedAddedFriend;
            set
            {
                _selectedAddedFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public MeetingWrapper Meeting
        {
            get => _meeting;
            private set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }

        protected override void OnDeleteExecute()
        {
            var result = _messageDialogService
                .ShowOkCancelDialog($"Do you want to delete the meeting {Meeting.Title}?", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _meetingRepository.Remove(Meeting.Model);
                _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async Task OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            RaiseDetailSaveEvent(Meeting.Id, Meeting.Title);
        }

        public override async Task LoadAsync(int? id)
        {
            var meeting = id.HasValue
                ? await _meetingRepository.GetByIdAsync(id.Value)
                : CreateNewMeeting();

            InitializeMeeting(meeting);

            _allFriends = await _meetingRepository.GetAllFriendsAsync();

            SetupPickList();
        }

        private void SetupPickList()
        {
            var meetingFriendIds = Meeting.Model.FriendMeetings.Select(f => f.FriendId); //todo: also need to get Friend in this model
            var addedFriends = _allFriends.Where(f => meetingFriendIds.Contains(f.Id)).OrderBy(f => f.FirstName);
            var availableFriends = _allFriends.Except(addedFriends).OrderBy(f => f.FirstName);

            AddedFriends.Clear();
            AvailableFriends.Clear();

            foreach (var addedFriend in addedFriends)
            {
                AddedFriends.Add(addedFriend);
            }

            foreach (var availableFriend in availableFriends)
            {
                AvailableFriends.Add(availableFriend);
            }
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _meetingRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Meeting.Id < 0)
            {
                //trigger the validation
                Meeting.Title = string.Empty;
            }
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }
    }
}