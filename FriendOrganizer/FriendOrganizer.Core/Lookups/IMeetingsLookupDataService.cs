using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Core.Models;

namespace FriendOrganizer.Core.Lookups
{
    public interface IMeetingsLookupDataService
    {
        Task<List<LookupItem>> GetMeetingLookupAsync();
    }
}