using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardlessAPI.Data.Models
{
    [Table("Business")]
    public class Business : ApplicationUser
    {
        [Required]
        public string Name { get; set; }
    }
}
