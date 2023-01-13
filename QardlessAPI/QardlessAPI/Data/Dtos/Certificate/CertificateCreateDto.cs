using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Certificate
{
    public class CertificateCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string QrCodeUri { get; set; }

        [Required]
        public string PdfUri { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public bool Expires { get; set; }

        public DateTime ExpiryDate { get; set; }

        [Required]
        public Guid EndUserId { get; set; }

        [Required]
        public Guid BusinessId { get; set; }


    }
}
