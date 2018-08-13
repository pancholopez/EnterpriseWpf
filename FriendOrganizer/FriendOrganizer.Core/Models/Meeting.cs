using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Core.Models
{
    public class Meeting
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ICollection<FriendMeetings> FriendMeetings { get; set; }

        public Meeting()
        {
            FriendMeetings = new Collection<FriendMeetings>();
        }
    }
}