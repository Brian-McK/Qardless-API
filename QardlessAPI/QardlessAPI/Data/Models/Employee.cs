using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardlessAPI.Data.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("BusinessId")]
        public Guid BusinessId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string ContactNumber { get; set; }

        [Required]
        public int PrivilegeLevel { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime LastLoginDate { get; set; }

    }
}
