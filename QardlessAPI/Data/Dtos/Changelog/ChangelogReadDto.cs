namespace QardlessAPI.Data.Dtos.Changelog
{
    public class ChangelogReadDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public bool WasRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
