using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QardlessAPI.Data.Models
{
    public class Certificate
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid CourseId { get; set; }
        
        [ForeignKey("CourseId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual Course Course { get; set; }
        
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
