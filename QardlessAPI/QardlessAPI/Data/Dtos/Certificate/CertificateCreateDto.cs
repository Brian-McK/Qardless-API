using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Certificate
{
    public class CertificateCreateDto
    {
        public Guid BusinessId { get; set; }
        public Guid CourseId { get; set; }
        
        public string EndUserEmail { get; set; }

        public string CertNumber { get; set; }

        public string PdfUrl { get; set; }

    }
}
