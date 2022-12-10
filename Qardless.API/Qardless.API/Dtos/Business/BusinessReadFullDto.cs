namespace Qardless.API.Dtos.Business
{
    public class BusinessReadFullDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
