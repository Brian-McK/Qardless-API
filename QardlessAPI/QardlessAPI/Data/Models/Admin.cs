using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Models
{
    public class Admin : ApplicationUser
    {
        [Required]
        public string Name { get; set; }
    }
}
