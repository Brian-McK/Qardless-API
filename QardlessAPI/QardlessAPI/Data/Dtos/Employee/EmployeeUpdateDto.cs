using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Employee
{
    public class EmployeeUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool EmailVerified { get; set; }

        [Required]
        public string Password { get; set; }

        public string? PhoneMobile { get; set; }

        public bool? PhoneMobileVerified { get; set; }

        [Required]
        public int PrivilegeLevel { get; set; }

        public DateTime LastLoginDate { get; set; }

        [Required]
        public Guid BusinessId { get; set; }
    }
}
