using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.FlaggedIssue
{
    public class FlaggedIssueUpdateDto
    {
        [Required]
        public bool WasRead { get; set; }

    }
}
