namespace QardlessAPI.Data.Dtos.Employee
{
    public class EmployeeReadPartialDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneMobile { get; set; }
        public Guid BusinessId { get; set; }
    }
}
