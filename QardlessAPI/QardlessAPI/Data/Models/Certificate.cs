using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardlessAPI.Data.Models
{
    public class Certificate
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid CourseId { get; set; }
        
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        
        [Required]
        public Guid EndUserId { get; set; }
        
        [ForeignKey("EndUserId")]
        public virtual EndUser EndUser { get; set; }

        [Required]
        public string CertNumber { get; set; }

        [Required]
        public string PdfUrl { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
