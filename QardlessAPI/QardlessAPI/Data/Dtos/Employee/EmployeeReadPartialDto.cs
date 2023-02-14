namespace QardlessAPI.Data.Dtos.Employee
{
    public class EmployeeReadPartialDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }
        public Guid BusinessId { get; set; }
    }
}
