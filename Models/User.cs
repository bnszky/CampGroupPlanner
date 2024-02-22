using System.ComponentModel.DataAnnotations;

namespace CampGroupPlanner.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(25)]
        public string Nick { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
       
    }
}
