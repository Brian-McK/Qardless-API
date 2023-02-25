using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardlessAPI.Data.Models
{
    public class Certificate
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("CourseId")]
        public Guid CourseId { get; set; }

        [Required]
        [ForeignKey("EndUserId")]
        public Guid EndUserId { get; set; }

        [Required]
        public string CertNumber { get; set; }

        [Required]
        public string PdfUrl { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
