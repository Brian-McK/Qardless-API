namespace QardlessAPI.Data.Dtos.FlaggedIssue
{
    public class FlaggedIssueReadDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public bool WasRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
