using Qardless.API.Models;

namespace Qardless.API.Services
{
    public interface IQardlessAPIRepo
    {
        bool SaveChanges();

        #region Admin
        IEnumerable<Admin> GetAdmins();
        Admin? GetAdmin(Guid adminId);
        void CreateAdmin(Admin admin);
        void UpdateAdmin(Admin admin);
        void DeleteAdmin(Admin admin);
        #endregion

        #region Business
        IEnumerable<Business> GetBusinesses();
        Business? GetBusiness(Guid businessId);
        void CreateBusiness(Business business);
        void UpdateBusiness(Business business);
        void DeleteBusiness(Business business);
        #endregion

        #region Certificate
        IEnumerable<Certificate> GetCertificates();
        Certificate? GetCertificate(Guid certificateId);
        void CreateCertificate(Certificate certificate);
        void UpdateCertificate(Certificate certificate);
        void DeleteCertificate(Certificate certificate);
        #endregion

        #region Changelog
        IEnumerable<Changelog> GetChangelogs();
        Changelog? GetChangelog(Guid changelogId);
        void CreateChangelog(Changelog changelog);
        void UpdateChangelog(Changelog changelog);
        void DeleteChangelog(Changelog changelog);
        #endregion

        #region Employee
        IEnumerable<Employee> GetEmployee();
        Changelog? GetEmployee(Guid employeeId);
        void CreateEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        #endregion

        #region EndUser
        IEnumerable<EndUser> GetEndUsers();
        EndUser? GetEndUser(Guid endUserId);
        void CreateEndUser(EndUser endUser);
        void UpdateEndUser(EndUser endUser);
        void DeleteEndUser(EndUser endUser);
        #endregion
    }
}
