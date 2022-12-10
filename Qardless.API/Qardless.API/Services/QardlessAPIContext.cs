using Microsoft.EntityFrameworkCore;
using Qardless.API.Models;

namespace Qardless.API.Services
{

    /*
     * NOTE: To create a Local SQL Server DB
     *      1)  Download and install MS SQL Server Management Studio
     *      2)  Open CMD or Powershell
     *      3)  Write and run command "sqllocaldb create "Local"
     *      4)  Open MS SQL Server Managment Studio
     *      5)  Server name: "(localdb)\Local", Auth: Windows Auth
     *      6)  In CMD or Powershell open the Qardless.API folder that
     *          is beside the sln file.
     *      7)  If there are no migrations:
     *              Write and run command "dotnet ef migrations add InitialMigration"
     *          Otherwise:
     *              Write and run command "dotnet ef database update"
     *      8)  You should then see the database and tables created in MS SQL Server Management Studio
     */
    public class QardlessAPIContext : DbContext
    {
        public QardlessAPIContext(DbContextOptions<QardlessAPIContext> opt) : base(opt)
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
