namespace FriendOrganizer.Core.Models
{
    public class FriendMeetings
    {
        public int FriendId { get; set; }
        public Friend Friend { get; set; }

        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}