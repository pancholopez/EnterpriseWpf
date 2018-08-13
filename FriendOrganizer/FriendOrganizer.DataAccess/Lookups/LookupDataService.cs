using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.Core.Lookups;
using FriendOrganizer.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FriendOrganizer.DataAccess.Lookups
{
    public class LookupDataService : 
        IFriendLookupDataService, 
        IProgrammingLanguageLookupDataService, 
        IMeetingsLookupDataService
    {
        private readonly Func<FriendOrganizerDbContext> _contextCreator;

        public LookupDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetFriendLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Friends.AsNoTracking().Select(friend => new LookupItem
                    {
                        Id = friend.Id,
                        DisplayMember = $"{friend.FirstName} {friend.LastName}"
                    }).ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.ProgrammingLanguages.AsNoTracking()
                    .Select(item => new LookupItem
                    {
                        Id = item.Id,
                        DisplayMember = item.Name
                    }).ToListAsync();
            }
        }

        public async Task<List<LookupItem>> GetMeetingLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                var items = await ctx.Meetings.AsNoTracking()
                    .Select(m => new LookupItem
                    {
                        Id = m.Id,
                        DisplayMember = m.Title
                    }).ToListAsync();
                return items;
            }
        }
    }
}