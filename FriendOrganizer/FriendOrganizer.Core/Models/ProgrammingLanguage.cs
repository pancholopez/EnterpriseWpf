using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Core.Models
{
    public class ProgrammingLanguage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}