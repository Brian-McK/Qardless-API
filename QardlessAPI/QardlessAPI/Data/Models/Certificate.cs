using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardlessAPI.Data.Models
{
    public class Certificate
    {
        [Key]
        public Guid Id { get; set; }

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
        public DateTime CreatedDate { get; set; }

        [Required]
        [ForeignKey("EndUserId")]
        public EndUser EndUser { get; set; }
        public Guid EndUserId { get; set; }

        [Required]
        [ForeignKey("BusinessId")]
        public Business Business { get; set; }
        public Guid BusinessId { get; set; }
    }
}
