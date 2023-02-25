namespace QardlessAPI.Data.Dtos.Certificate
{
    public class CertificateReadPartialDto
    {
        public Guid Id { get; set; }

        public Guid CourseId { get; set; }

        public Guid EndUserId { get; set; }

    }
}
