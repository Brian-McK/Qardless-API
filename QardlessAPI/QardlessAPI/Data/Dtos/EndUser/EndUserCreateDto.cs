using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.EndUser
{
    public class EndUserCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool EmailVerified { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string? ContactNumber { get; set; }
    }
}
