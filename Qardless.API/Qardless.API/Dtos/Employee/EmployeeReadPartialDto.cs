namespace Qardless.API.Dtos.Employee
{
    public class EmployeeReadPartialDto
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneMobile { get; set; }
        public Guid BusinessId { get; set; }
    }
}
