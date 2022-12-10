namespace Qardless.API.Dtos.EndUser
{
    public class EndUserReadFullDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string? PhoneMobile { get; set; }
        public bool? PhoneMobileVerified { get; set; }
        public string? PhoneHome { get; set; }
        public string AddressCode { get; set; }
        public string AddressDetailed { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }


    }
}
