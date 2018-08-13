using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Core.Models;

namespace FriendOrganizer.Core.Repositories
{
    public interface IMeetingRepository : IGenericRepository<Meeting>
    {
        Task<List<Friend>> GetAllFriendsAsync();
        Task ReloadFriendAsync(int id);
    }
}