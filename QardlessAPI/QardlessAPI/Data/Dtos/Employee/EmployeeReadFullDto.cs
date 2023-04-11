namespace QardlessAPI.Data.Dtos.Employee
{
    public class EmployeeReadFullDto
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int PrivilegeLevel { get; set; }
    }
}
