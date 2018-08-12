using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    public class Friend
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(50, ErrorMessage = "{0} maximum length is {1} characters.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "{0} is not valid.")]
        public string Email { get; set; }

        public int? FavoriteLanguageId { get; set; }

        //todo: check if this can be deleted
        public ProgrammingLanguage FavoriteLanguage { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<FriendPhoneNumber> PhoneNumbers { get; set; }

        public ICollection<FriendMeetings> FriendMeetings { get; set; }

        public Friend()
        {
            PhoneNumbers = new Collection<FriendPhoneNumber>();
            FriendMeetings = new Collection<FriendMeetings>();
        }
    }
}
