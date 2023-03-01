namespace QardlessAPI.Data.Dtos.Course
{
    public class CourseReadDto
    {
        public Guid BusinessId { get; set; }
        public string Title { get; set; }
        public string CourseDate { get; set; }
        public string Expiry { get; set; }
    }
}
