namespace QardlessAPI.Data.Dtos.EndUser
{
    public class EndUserReadFullDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string? ContactNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginDate { get; set; }
        public List<Models.Certificate> EndUserCerts { get; set; }
    }
}
