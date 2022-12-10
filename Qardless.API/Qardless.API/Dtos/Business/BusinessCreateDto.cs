using System.ComponentModel.DataAnnotations;

namespace Qardless.API.Dtos.Business
{
    public class BusinessCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Phone { get; set; }
    }
}
