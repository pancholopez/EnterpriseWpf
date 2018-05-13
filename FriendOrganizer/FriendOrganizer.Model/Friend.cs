﻿using System.ComponentModel;
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
    }
}
