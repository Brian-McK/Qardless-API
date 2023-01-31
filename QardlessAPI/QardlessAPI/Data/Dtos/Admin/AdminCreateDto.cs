using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Admin
{
    public class AdminCreateDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool EmailVerified { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        public string? PhoneMobile { get; set; }
        
        [Required]
        public bool PhoneMobileVerified { get; set; }
    }
}
