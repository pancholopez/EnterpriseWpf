using Prism.Events;

namespace FriendOrganizer.UI.Event
{
    public class AfterFriendSaveEvent : PubSubEvent<AfterFriendSaveEventArgs>
    {

    }

    public class AfterFriendSaveEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}