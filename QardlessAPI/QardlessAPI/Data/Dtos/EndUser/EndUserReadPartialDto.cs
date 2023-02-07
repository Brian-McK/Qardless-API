namespace QardlessAPI.Data.Dtos.EndUser
{
    public class EndUserReadPartialDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }
        public bool isLoggedin { get; set; }
    }
}
