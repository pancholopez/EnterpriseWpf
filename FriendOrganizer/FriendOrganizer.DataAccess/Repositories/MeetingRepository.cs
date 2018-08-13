using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.Core.Models;
using FriendOrganizer.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FriendOrganizer.DataAccess.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDbContext>, IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDbContext context) : base(context)
        {
        }

        public override async Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings
                .Include(m => m.FriendMeetings)
                .SingleAsync(m => m.Id == id);
        }

        public async Task<List<Friend>> GetAllFriendsAsync()
        {
            return await Context.Set<Friend>().ToListAsync();
        }

        public async Task ReloadFriendAsync(int id)
        {
            var dbEntityEntry = Context.ChangeTracker.Entries<Friend>()
                .SingleOrDefault(db => db.Entity.Id == id);
            if (dbEntityEntry != null)
            {
                await dbEntityEntry.ReloadAsync();
            }
        }
    }
}