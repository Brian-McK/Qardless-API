using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardlessAPI.Data.Models
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("BusinessId")]
        public Guid BusinessId { get; set; }

        public string Title { get; set; }

        public DateOnly CourseDate { get; set; }

        public DateOnly Expiry { get; set; }

    }
}
