namespace QardlessAPI.Data.Dtos.EndUser
{
    public class EndUserReadPartialDto
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneMobile { get; set; }
        public string AddressCode { get; set; }
        public string AddressDetailed { get; set; }
    }
}
