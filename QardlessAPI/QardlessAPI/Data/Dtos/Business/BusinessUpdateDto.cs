using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Business
{
    public class BusinessUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
