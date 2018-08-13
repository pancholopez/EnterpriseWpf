using System.Threading.Tasks;
using FriendOrganizer.Core.Models;

namespace FriendOrganizer.Core.Repositories
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {
        Task<bool> HasMeetingsASync(int friendId);
        void RemovePhoneNumber(FriendPhoneNumber phoneNumber);
    }
}