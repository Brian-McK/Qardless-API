using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Models
{
    public class Business
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string Contact { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
