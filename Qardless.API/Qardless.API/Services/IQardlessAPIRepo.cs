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
        #endregion

        #region Certificate
        #endregion

        #region Changelog
        #endregion

        #region Employee
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
