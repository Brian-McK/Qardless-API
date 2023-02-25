namespace QardlessAPI.Data.Dtos.Certificate
{
    public class CertificateReadFullDto
    {
        public Guid Id { get; set; }
        
        public Guid CourseId { get; set; }

        public Guid EndUserId { get; set; }

        public string CertNumber { get; set; }

        public string PdfUrl { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
