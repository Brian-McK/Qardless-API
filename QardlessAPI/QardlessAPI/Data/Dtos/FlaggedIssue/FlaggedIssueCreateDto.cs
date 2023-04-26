using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.FlaggedIssue
{
    public class FlaggedIssueCreateDto
    {
        public Guid CertificateId { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
