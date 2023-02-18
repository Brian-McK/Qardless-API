using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Models
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool EmailVerified { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string ContactNumber { get; set; }

        [Required]
        public bool ContactNumberVerified { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime LastLoginDate { get; set; }
    
    }
}
