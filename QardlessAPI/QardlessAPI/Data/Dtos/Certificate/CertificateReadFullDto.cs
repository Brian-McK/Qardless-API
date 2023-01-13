namespace QardlessAPI.Data.Dtos.Certificate
{
    public class CertificateReadFullDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string QrCodeUri { get; set; }
        public string PdfUri { get; set; }
        public string SerialNumber { get; set; }
        public bool Expires { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid EndUserId { get; set; }
        public Guid BusinessId { get; set; }
    }
}
