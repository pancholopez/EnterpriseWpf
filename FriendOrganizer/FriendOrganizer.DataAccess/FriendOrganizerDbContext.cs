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
            //DataSeeder.SeedMeetings(this);
        }

        public FriendOrganizerDbContext(DbContextOptions<FriendOrganizerDbContext> options)
        :base(options)
        {
            DataSeeder.SeedFriends(this);
            DataSeeder.SeedProgrammingLanguages(this);
        }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<FriendPhoneNumber> FriendPhoneNumbers { get; set; }
        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=FriendOrganizerDb;Trusted_Connection=True;");

            }
            //todo: remove pluralization
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FriendMeetings>()
                .HasKey(x => new {x.FriendId, x.MeetingId});

            modelBuilder.Entity<FriendMeetings>()
                .HasOne(fm => fm.Friend)
                .WithMany(m => m.FriendMeetings)
                .HasForeignKey(fm => fm.FriendId);

            modelBuilder.Entity<FriendMeetings>()
                .HasOne(fm => fm.Meeting)
                .WithMany(f => f.FriendMeetings)
                .HasForeignKey(fm => fm.MeetingId);
        }
    }
}