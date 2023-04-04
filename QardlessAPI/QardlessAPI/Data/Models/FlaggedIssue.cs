using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QardlessAPI.Data.Models
{
    public class FlaggedIssue
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public bool WasRead { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public Guid CertificateId { get; set; }

        [ForeignKey("CertificateId")]
        public virtual Certificate Certificate { get; set; }
    }
}
