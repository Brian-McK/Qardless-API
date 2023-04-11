namespace QardlessAPI.Data.Dtos.EndUser
{
    public class EndUserReadPartialDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
