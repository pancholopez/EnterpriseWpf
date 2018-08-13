using System.Threading.Tasks;
using FriendOrganizer.Core.Models;
using FriendOrganizer.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FriendOrganizer.DataAccess.Repositories
{
    public class ProgrammingLanguageRepository
        : GenericRepository<ProgrammingLanguage, FriendOrganizerDbContext>,
            IProgrammingLanguageRepository
    {
        public ProgrammingLanguageRepository(FriendOrganizerDbContext context) : base(context)
        {
        }

        public async Task<bool> IsReferencedByFriendAsync(int programmingLanguageId)
        {
            return await Context.Friends.AsNoTracking()
                .AnyAsync(f => f.FavoriteLanguageId == programmingLanguageId);
        }
    }
}