using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardlessAPI.Data.Models
{
    public class Certificate
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CourseTitle { get; set; }

        [Required]
        public string CertNumber { get; set; }

        [Required]
        public DateTime CourseDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public string PdfUrl { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        [ForeignKey("EndUserId")]
        public Guid EndUserId { get; set; }

        [Required]
        [ForeignKey("BusinessId")]
        public Business Business { get; set; }
        public Guid BusinessId { get; set; }
    }
}
