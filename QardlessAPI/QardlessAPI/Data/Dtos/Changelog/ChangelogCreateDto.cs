using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Changelog
{
    public class ChangelogCreateDto
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
