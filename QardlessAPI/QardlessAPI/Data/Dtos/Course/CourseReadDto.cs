namespace QardlessAPI.Data.Dtos.Course
{
    public class CourseReadDto
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public string Title { get; set; }
        public DateOnly CourseDate { get; set; }
        public DateOnly Expiry { get; set; }
    }
}
