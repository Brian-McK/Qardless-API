using System.ComponentModel.DataAnnotations;

namespace Qardless.API.Dtos.Changelog
{
    public class ChangelogUpdateDto
    {
        [Required]
        public bool WasRead { get; set; }

    }
}
