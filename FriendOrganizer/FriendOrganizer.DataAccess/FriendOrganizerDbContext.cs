using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace FriendOrganizer.DataAccess
{
    public class FriendOrganizerDbContext : DbContext
    {
        public FriendOrganizerDbContext()
        {
            //DataSeeder.SeedFriends(this);
            //DataSeeder.SeedProgrammingLanguages(this);
        }

        public FriendOrganizerDbContext(DbContextOptions<FriendOrganizerDbContext> options)
        :base(options)
        {
            DataSeeder.SeedFriends(this);
            DataSeeder.SeedProgrammingLanguages(this);
        }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=FriendOrganizerDb;Trusted_Connection=True;");

            }
            //todo: remove pluralization
        }
    }
}