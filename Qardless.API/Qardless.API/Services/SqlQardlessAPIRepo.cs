using Qardless.API.Dtos.EndUser;
using Qardless.API.Models;

namespace Qardless.API.Services
{
    public class SqlQardlessAPIRepo : IQardlessAPIRepo
    {
        private readonly QardlessAPIContext _context;

        public SqlQardlessAPIRepo(QardlessAPIContext context)
        {
            _context = context;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        #region Admin
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
        public IEnumerable<EndUser> GetEndUsers()
        {
            return _context.EndUsers.ToList();
        }

        public EndUser? GetEndUser(Guid endUserId)
        {
            return _context.EndUsers.FirstOrDefault(a => a.Id == endUserId);
        }

        public void CreateEndUser(EndUser endUser)
        {
            if (endUser == null)
                throw new ArgumentNullException(nameof(endUser));

            endUser.Id = Guid.NewGuid();
            endUser.CreatedDate = DateTime.Now;
            endUser.LastLoginDate = endUser.CreatedDate;

            _context.EndUsers.Add(endUser);
        }
        
        public void UpdateEndUser(EndUser endUser)
        {
            // no code in this implementation
        }

        public void DeleteEndUser(EndUser endUser)
        {
            if (endUser == null)
                throw new ArgumentNullException(nameof(endUser));

            _context.EndUsers.Remove(endUser);
        }
        #endregion
    }
}
