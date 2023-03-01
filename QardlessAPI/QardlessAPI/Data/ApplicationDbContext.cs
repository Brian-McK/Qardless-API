using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base()
        {
        }

        public ApplicationDbContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<FlaggedIssue> FlaggedIssues { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EndUser> EndUsers { get; set; }
    }
}
