using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Friend>> GetAllAsync()
        {
            //todo: remove seeder from here
            var ctx = _contextCreator();
            DataSeeder.SeedFriends(ctx);
            using (ctx)
            {
                return await ctx.Friends.AsNoTracking().ToListAsync();
            }
        }
    }
}