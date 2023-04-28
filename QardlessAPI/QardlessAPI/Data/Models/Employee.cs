using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardlessAPI.Data.Models
{
    public class Employee : ApplicationUser
    {
        [Required]
        public string Name { get; set; }

        // Needed to make nullable to avoid a on delete cascade. TODO: Look into changing constraints in modelbuilder in Dbcontext
        public string? BusinessId { get; set; }
        
        [ForeignKey("BusinessId")]
        public virtual Business? Business { get; set; }

        [Required]
        public int PrivilegeLevel { get; set; }
    }
}
