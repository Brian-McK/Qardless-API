using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Models
{
    public class Business
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Phone { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
