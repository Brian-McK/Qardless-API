using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Admin
{
    public class AdminUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool EmailVerified { get; set; }

        [Required]
        public string Password { get; set; }

        public string ContactNumber { get; set; }

        [Required]
        public bool ContactNumberVerified { get; set; }

        public DateTime LastLoginDate { get; set; }
    }
}
