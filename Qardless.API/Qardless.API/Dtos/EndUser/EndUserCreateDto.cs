using System.ComponentModel.DataAnnotations;

namespace Qardless.API.Dtos.EndUser
{
    public class EndUserCreateDto
    {
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

        public string? PhoneHome { get; set; }

        [Required]
        public string AddressCode { get; set; }

        [Required]
        public string AddressDetailed { get; set; }


    }
}
