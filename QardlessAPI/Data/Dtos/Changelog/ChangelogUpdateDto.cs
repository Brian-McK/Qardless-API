using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Changelog
{
    public class ChangelogUpdateDto
    {
        [Required]
        public bool WasRead { get; set; }

    }
}
