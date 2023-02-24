namespace QardlessAPI.Data.Dtos.Business
{
    public class BusinessReadFullDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
