using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardlessAPI.Data.Models
{
    public class Employee : ApplicationUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid BusinessId { get; set; }
        
        [ForeignKey("BusinessId")]
        public virtual Business Business { get; set; }

        [Required]
        public int PrivilegeLevel { get; set; }
    }
}
