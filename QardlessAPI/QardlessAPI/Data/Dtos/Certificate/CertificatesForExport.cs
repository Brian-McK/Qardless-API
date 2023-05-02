namespace QardlessAPI.Data.Dtos.Certificate
{
    public class CertificatesForExport
    {
        public Guid Id { get; set; }
        public Models.Course Course { get; set; }
        public string CertNumber { get; set; }
        public string PdfUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string EndUserName { get; set; }
        public string EndUserEmail { get; set; }
        public bool IsFrozen { get; set; }
    }
}
