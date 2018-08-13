using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Core.Models;

namespace FriendOrganizer.Core.Lookups
{
    public interface IProgrammingLanguageLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync();
    }
}