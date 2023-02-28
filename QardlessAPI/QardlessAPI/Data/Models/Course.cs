using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardlessAPI.Data.Models
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid BusinessId { get; set; }
        
        [ForeignKey("BusinessId")]
        public virtual Business Business { get; set; }

        public string Title { get; set; }

        public DateTime CourseDate { get; set; }

        public DateTime Expiry { get; set; }

    }
}
