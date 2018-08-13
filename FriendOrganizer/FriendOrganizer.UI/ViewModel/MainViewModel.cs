using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac.Features.Indexed;
using FriendOrganizer.Core.Services;
using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IIndex<string, IDetailViewModel> _detailViewModelCreator;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private IDetailViewModel _selectedDetailViewModel;

        public INavigationViewModel NavigationViewModel { get; }
        public IDetailViewModel SelectedDetailViewModel
        {
            get => _selectedDetailViewModel;
            set
            {
                _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }

        public ICommand CreateNewDetailCommand { get; }
        
        public ICommand OpenSingleDetailViewCommand { get; }

        public MainViewModel(INavigationViewModel navigationViewModel,
            IIndex<string, IDetailViewModel> detailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _detailViewModelCreator = detailViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;

            DetailViewModels = new ObservableCollection<IDetailViewModel>();

            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);
            _eventAggregator.GetEvent<AfterDetailClosedEvent>()
                .Subscribe(AfterDetailClosed);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            OpenSingleDetailViewCommand = new DelegateCommand<Type>(OnOpenSingleDetailViewExecute);
            NavigationViewModel = navigationViewModel;
        }

        private void AfterDetailClosed(AfterDetailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void RemoveDetailViewModel(int id, string viewModelName)
        {
            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == id
                                       && vm.GetType().Name == viewModelName);

            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            //doesn't make sense for TAB UI
            //if (SelectedDetailViewModel != null && SelectedDetailViewModel.HasChanges)
            //{
            //    var result =
            //        _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
            //    if (result == MessageDialogResult.Cancel) return;
            //}

            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == args.Id
                                       && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = _detailViewModelCreator[args.ViewModelName];
                try
                {
                    await detailViewModel.LoadAsync(args.Id);
                }
                catch (Exception)
                {
                    _messageDialogService
                        .ShowInfoDialog("Could not load the entity, maybe it was deleted in the meantime " +
                                        "by another user. The navigation will automatically refresh.");
                    await NavigationViewModel.LoadAsync();
                    return;
                }
                DetailViewModels.Add(detailViewModel);
            }

            SelectedDetailViewModel = detailViewModel;

        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = viewModelType.Name });
        }

        private void OnOpenSingleDetailViewExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs
            {
                Id = -1,
                ViewModelName = viewModelType.Name
            });
        }
    }
}