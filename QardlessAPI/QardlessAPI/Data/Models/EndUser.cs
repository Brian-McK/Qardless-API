using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QardlessAPI.Data.Models
{
    [Table("EndUsers")]
    public class EndUser : ApplicationUser
    {
        [Required]
        public string Name { get; set; }

        // Note: Changed from List<> to Collection, as List<> is unnessaraly limiting.
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual List<Certificate> EndUserCerts { get; set; }
    }
}
