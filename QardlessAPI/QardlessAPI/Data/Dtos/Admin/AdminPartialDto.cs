namespace QardlessAPI.Data.Dtos.Admin
{
    public class AdminPartialDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}
