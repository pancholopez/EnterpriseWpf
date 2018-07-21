using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {
        Task<bool> HasMeetingsASync(int friendId);
        void RemovePhoneNumber(FriendPhoneNumber phoneNumber);
    }
}