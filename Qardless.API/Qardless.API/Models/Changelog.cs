using System.ComponentModel.DataAnnotations;

namespace Qardless.API.Models
{
    public class Changelog
    {
        [Key]
        public Guid Id { get; set; }

        // NOTE: Could be an Enum
        [Required]
        public string Type { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public bool WasRead { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
