using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Business
{
    public class BusinessCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Contact { get; set; }
    }
}
