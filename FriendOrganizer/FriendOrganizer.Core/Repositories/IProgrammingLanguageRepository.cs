using System.Threading.Tasks;
using FriendOrganizer.Core.Models;

namespace FriendOrganizer.Core.Repositories
{
    public interface IProgrammingLanguageRepository
    : IGenericRepository<ProgrammingLanguage>
    {
        Task<bool> IsReferencedByFriendAsync(int programmingLanguageId);
    }
}