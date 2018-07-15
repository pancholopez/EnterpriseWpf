using System;
using System.Collections.Generic;
using System.Linq;
using FriendOrganizer.Model;

namespace FriendOrganizer.DataAccess
{
    ///Idea From: https://www.learnentityframeworkcore.com/migrations/seeding
    public class DataSeeder
    {
        public static void SeedFriends(FriendOrganizerDbContext context)
        {
            if (!context.Friends.Any())
            {
                var friends = new List<Friend>
                {
                    new Friend {FirstName = "Francisco", LastName = "Lopez", Email = "pancho@mail.com"},
                    new Friend {FirstName = "Irma", LastName = "Estrada", Email = "irma@mail.com"},
                    new Friend {FirstName = "Christopher", LastName = "Lopez", Email = "chris@email.com"},
                    new Friend {FirstName = "Fidel", LastName = "Razo", Email = "fidel@mail.com"}
                };
                context.AddRange(friends);
                context.SaveChanges();
            }
        }

        public static void SeedProgrammingLanguages(FriendOrganizerDbContext context)
        {
            if (!context.ProgrammingLanguages.Any())
            {
                var languages = new List<ProgrammingLanguage>
                {
                    new ProgrammingLanguage {Name = "C#"},
                    new ProgrammingLanguage {Name = "Java"},
                    new ProgrammingLanguage {Name = "PHP"},
                    new ProgrammingLanguage {Name = "Node"},
                    new ProgrammingLanguage {Name = "JavaScript"}
                };
                context.AddRange(languages);
                context.SaveChanges();
            }
        }

        public static void SeedMeetings(FriendOrganizerDbContext context)
        {
            if (!context.Meetings.Any())
            {
                var friends = context.Friends.Take(2).ToArray();
                var meeting = new Meeting
                {
                    DateFrom = DateTime.Today.AddHours(10),
                    DateTo = DateTime.Today.AddHours(20),
                    Title = "Test Meeting",

                };
                context.Add(meeting);
                context.SaveChanges();
                var friendMeeting1 = new FriendMeetings
                {
                    FriendId = friends[0].Id,
                    Friend = friends[0],
                    MeetingId = meeting.Id,
                    Meeting = meeting
                };
                var friendMeeting2 = new FriendMeetings
                {
                    FriendId = friends[1].Id,
                    Friend = friends[1],
                    MeetingId = meeting.Id,
                    Meeting = meeting
                };

                context.AddRange(friendMeeting1, friendMeeting2);
                context.SaveChanges();
            }
        }
    }
}