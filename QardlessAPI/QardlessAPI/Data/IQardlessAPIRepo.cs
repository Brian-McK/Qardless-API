using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Bson;
//using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data
{
    public interface IQardlessAPIRepo
    {
        bool SaveChanges();

        #region Admin
        Task<IEnumerable<Admin?>> GetAdmins();
        Task<Admin?> GetAdmin(Guid id);
        void PutAdmin(Guid id, Admin? admin);
        void PatchAdmin(Guid id, Admin? admin);
        void PostAdmin(Admin? admin);
        void DeleteAdmin(Admin? admin);
        #endregion

        #region Business
        Task<IEnumerable<Business?>> GetBusinesses();
        Task<Business?> GetBusiness(Guid id);
        void PutBusiness(Guid id, Business? business);
        void PatchBusiness(Guid id, Business? business);
        void PostBusiness(Business? business);
        void DeleteBusiness(Business? business);
        #endregion

        #region Certificate
        Task<IEnumerable<Certificate?>> GetCertificates();
        Task<IEnumerable<Certificate?>> GetCertificatesByEndUserId(Guid id);
        Task<Certificate?> GetCertificate(Guid id);
        void PutCertificate(Guid id, Certificate? certificate);
        void PatchCertificate(Guid id, Certificate? certificate);
        void PostCertificate(Certificate? certificate);
        void DeleteCertificate(Certificate? certificate);
        #endregion

        #region Changelog
        Task<IEnumerable<Changelog?>> GetChangelogs();
        Task<Changelog?> GetChangelog(Guid id);
        void PutChangelog(Guid id, Changelog? changelog);
        void PatchChangelog(Guid id, Changelog? changelog);
        void PostChangelog(Changelog? changelog);
        void DeleteChangelog(Changelog? changelog);
        #endregion

        #region Employee
        Task<IEnumerable<Employee?>> GetEmployees();
        Task<IEnumerable<Employee?>> GetEmployeesByBusinessId(Guid id);
        Task<Employee?> GetEmployee(Guid id);
        void PutEmployee(Guid id, Employee? employee);
        void PatchEmployee(Guid id, Employee? employee);
        void PostEmployee(Employee? employee);
        void DeleteEmployee(Employee? employee);
        #endregion

        #region EndUser
        Task<IEnumerable<EndUser?>> GetEndUsers();
        Task<EndUser?> GetEndUser(Guid id);
        //Task<EndUser?> GetEndUserByEmail(EndUserLoginDto loginDto);
        void PutEndUser(Guid id, EndUser endUser);
        void PatchEndUser(Guid id, EndUser endUser);
        void PostEndUser(EndUser endUser);
        void DeleteEndUser(EndUser endUser);

        #endregion
    }
}
