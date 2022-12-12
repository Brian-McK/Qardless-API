using Qardless.API.Models;

namespace Qardless.API.Services
{
    public interface IEndUserRepository
    {
        IEnumerable<EndUser> GetEndUsers();
        EndUser GetEndUser(Guid endUserId);
        void AddEndUser(EndUser endUser);
        void UpdateEndUser(EndUser endUser);
        void DeleteEndUser(EndUser endUser);
        bool EndUserExists(Guid endUserId);
        bool Save();
    }
}
