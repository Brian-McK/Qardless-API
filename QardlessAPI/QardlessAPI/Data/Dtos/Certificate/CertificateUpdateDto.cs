using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Certificate
{
    public class CertificateUpdateDto
    {
        [Required]
        public Guid CourseId { get; set; }

        [Required]
        public Guid EndUserId { get; set; }

        [Required]
        public string CertNumber { get; set; }

        public string PdfUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
