using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QardlessAPI.Data.Models
{
    public class EndUser
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

        public string? ContactNumber { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime LastLoginDate { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual List<Certificate> EndUserCerts { get; set; }
    }
}
