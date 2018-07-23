using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public interface IDetailViewModel
    {
        int Id { get; }
        Task LoadAsync(int id);
        bool HasChanges { get; }
    }
}