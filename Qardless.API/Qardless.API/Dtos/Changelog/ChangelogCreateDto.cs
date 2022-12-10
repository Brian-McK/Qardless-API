using System.ComponentModel.DataAnnotations;

namespace Qardless.API.Dtos.Changelog
{
    public class ChangelogCreateDto
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
