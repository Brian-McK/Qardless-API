using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data.Models;
using System.Diagnostics.Metrics;

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
        public DbSet<Changelog> Changelogs { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EndUser> EndUsers { get; set; }

    }
}
