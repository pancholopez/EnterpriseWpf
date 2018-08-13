using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Core.Models
{
    public class FriendPhoneNumber
    {
        public int Id { get; set; }

        [Phone]
        [Required]
        public string Number { get; set; }

        public int FriendId { get; set; }

        public Friend Friend { get; set; }
    }
}