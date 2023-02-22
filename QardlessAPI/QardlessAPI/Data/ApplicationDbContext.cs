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
        public DbSet<Changelog> Changelogs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EndUser> EndUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EndUser>()
                .HasMany(c => c.EndUserCerts)
                .WithOne()
                .HasForeignKey(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EndUser>()
                .Navigation(c => c.EndUserCerts)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
        }
    }
}
