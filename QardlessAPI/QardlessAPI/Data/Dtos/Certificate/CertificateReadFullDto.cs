namespace QardlessAPI.Data.Dtos.Certificate
{
    public class CertificateReadFullDto
    {
        public Guid Id { get; set; }
        public string CourseTitle { get; set; }
        public string CertNumber { get; set; }
        public DateTime CourseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string PdfUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid EndUserId { get; set; }
        public Guid BusinessId { get; set; }
    }
}
