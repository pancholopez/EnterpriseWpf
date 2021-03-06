﻿using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : GenericRepository<Friend, FriendOrganizerDbContext>, IFriendRepository
    {
        public FriendRepository(FriendOrganizerDbContext context) : base(context)
        {
        }

        public override async Task<Friend> GetByIdAsync(int friendId)
        {
            return await Context.Friends
                .Include(f => f.PhoneNumbers)
                .SingleAsync(friend => friend.Id == friendId);
        }

        public void RemovePhoneNumber(FriendPhoneNumber phoneNumber)
        {
            Context.FriendPhoneNumbers.Remove(phoneNumber);
        }

        public async Task<bool> HasMeetingsASync(int friendId)
        {
            return await Context.Meetings.AsNoTracking()
                .Include(m => m.FriendMeetings)
                .AnyAsync(m => m.FriendMeetings.Any(f => f.FriendId == friendId));
        }
    }
}