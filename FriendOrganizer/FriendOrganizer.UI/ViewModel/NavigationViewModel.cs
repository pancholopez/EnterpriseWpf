using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : INavigationViewModel
    {
        private readonly IFriendLookupDataService _friendLookupDataService;
        public ObservableCollection<LookupItem> Friends { get; }

        public NavigationViewModel(IFriendLookupDataService friendLookupDataService)
        {
            _friendLookupDataService = friendLookupDataService;
            Friends = new ObservableCollection<LookupItem>();
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupDataService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(item);
            }
        }
    }
}