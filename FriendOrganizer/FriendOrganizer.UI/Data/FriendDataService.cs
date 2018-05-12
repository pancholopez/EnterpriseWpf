using System;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        private readonly Func<FriendOrganizerDbContext> _contextCreator;

        public FriendDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<Friend> GetByIdAsync(int friendId)
        {
            using ( var ctx = _contextCreator())
            {
                //todo: remove seeder from here
                DataSeeder.SeedFriends(ctx);
                return await ctx.Friends.AsNoTracking()
                    .SingleAsync(friend => friend.Id == friendId);
            }
        }
    }
}