using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardlessAPI.Data.Models
{
    public class FlaggedIssue
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public bool WasRead { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public Guid EndUserId { get; set; }
        
        [ForeignKey("EndUserId")]
        public virtual EndUser EndUser { get; set; }
    }
}
