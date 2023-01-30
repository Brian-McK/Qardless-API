using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Certificate
{
    public class CertificateCreateDto
    {
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
        public Guid EndUserId { get; set; }

        [Required]
        public Guid BusinessId { get; set; }


    }
}
