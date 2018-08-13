using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Core.Services;
using FriendOrganizer.UI.Event;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        protected readonly IEventAggregator EventAggregator;
        protected readonly IMessageDialogService MessageDialogService;
        private string _title;
        private bool _hasChanges;

        public int Id { get; protected set; }

        public string Title
        {
            get => _title;
            protected set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public bool HasChanges
        {
            get => _hasChanges;
            protected set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CloseDetailViewCommand { get; }

        protected DetailViewModelBase(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            EventAggregator = eventAggregator;
            MessageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            CloseDetailViewCommand = new DelegateCommand(OnCloseDetailViewExecute);
        }

        protected abstract void OnDeleteExecute();
        protected abstract bool OnSaveCanExecute();
        protected abstract void OnSaveExecute();

        protected virtual void RaiseCollectionSavedEvent()
        {
            EventAggregator.GetEvent<AfterCollectionSavedEvent>()
                .Publish(new AfterCollectionSavedEventArgs
                {
                    ViewModelName = GetType().Name
                });
        }

        protected virtual void OnCloseDetailViewExecute()
        {
            if (HasChanges)
            {
                var result = MessageDialogService.ShowOkCancelDialog(
                    "You've made changes. CLose this item?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            EventAggregator.GetEvent<AfterDetailClosedEvent>()
                .Publish(new AfterDetailClosedEventArgs
                {
                    Id = Id,
                    ViewModelName = GetType().Name
                });
        }

        public abstract Task LoadAsync(int id);

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Publish(new AfterDetailDeletedEventArgs
                {
                    Id = modelId,
                    ViewModelName = this.GetType().Name
                });
        }

        protected virtual void RaiseDetailSaveEvent(int modelId, string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailSavedEvent>()
                .Publish(new AfterDetailSavedEventArgs
                {
                    Id = modelId,
                    DisplayMember = displayMember,
                    ViewModelName = this.GetType().Name
                });
        }

        protected async Task SaveWithOptimisticConcurrencyAsync(Func<Task> saveFunc, 
            Action afterSaveAction)
        {
            try
            {
                await saveFunc();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                var databaseValues = exception.Entries.Single().GetDatabaseValues();
                if (databaseValues == null)
                {
                    MessageDialogService.ShowInfoDialog("The entity has been deleted by another user.");
                    RaiseDetailDeletedEvent(Id);
                    return;
                }

                var result = MessageDialogService.ShowOkCancelDialog(
                    "The entity has changed in the meantime by someone else. " +
                    "Click OK to save your changes anyway, click Cancel " +
                    "to reload the entity from the database.", "Question");

                if (result == MessageDialogResult.Ok)
                {
                    //update the original values with database-values
                    var entry = exception.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    await saveFunc();
                }
                else
                {
                    //reload entity from database
                    await exception.Entries.Single().ReloadAsync();
                    await LoadAsync(Id);
                }
            }

            afterSaveAction();
        }
    }
}