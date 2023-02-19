namespace QardlessAPI.Data.Dtos.Business
{
    public class BusinessReadPartialDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
    }
}
