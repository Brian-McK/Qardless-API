using System.ComponentModel.DataAnnotations;

namespace Qardless.API.Models
{
    public class EndUser
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

        // TODO: Ask Fergal if a user MUST have a mobile phone number
        public string? PhoneMobile { get; set; }

        public bool? PhoneMobileVerified { get; set; }

        public string? PhoneHome { get; set; }

        [Required]
        public string AddressCode { get; set; }

        [Required]
        public string AddressDetailed { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime LastLoginDate { get; set; }
    }
}
