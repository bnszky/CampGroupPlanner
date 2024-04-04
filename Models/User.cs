using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CampGroupPlanner.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Nick { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
		[NotNull]
		public string FirstName { get; set; }

        [MaxLength(50)]
		[NotNull]
		public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [NotNull]
        public string HashPassword { get; set; }
        public virtual List<Review>? Reviews { get; set; }
       
    }
}
