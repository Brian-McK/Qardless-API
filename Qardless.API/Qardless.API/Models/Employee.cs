using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qardless.API.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool EmailVerified { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string? PhoneMobile { get; set; }

        public bool? PhoneMobileVerified { get; set; }

        // NOTE: Could be an Enum
        [Required]
        public int PrivilegeLevel { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        [Required]
        [ForeignKey("BusinessId")]
        public Business Business { get; set; }
        public Guid BusinessId { get; set; }

    }
}
