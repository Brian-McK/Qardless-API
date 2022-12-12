using Qardless.API.Models;

namespace Qardless.API.Services
{
    public class EndUserRepository : IEndUserRepository, IDisposable
    {
        private readonly QardlessAPIContext _context;

        public EndUserRepository(QardlessAPIContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<EndUser> GetEndUsers()
        {
            return _context.EndUsers.ToList<EndUser>();
        }

        public EndUser GetEndUser(Guid endUserId)
        {
            if (endUserId == Guid.Empty)
                throw new ArgumentNullException(nameof(endUserId));
            
            return _context.EndUsers.FirstOrDefault(a => a.Id == endUserId);
        }

        public void AddEndUser(EndUser endUser)
        {
            if (endUser == null)
                throw new ArgumentNullException(nameof(endUser));

            endUser.Id = Guid.NewGuid();

            _context.EndUsers.Add(endUser);
        }

        public bool EndUserExists(Guid endUserId)
        {
            if (endUserId == Guid.Empty)
                throw new ArgumentNullException(nameof(endUserId));

            return _context.EndUsers
                .Any(a => a.Id == endUserId);
        }

        public void DeleteEndUser(EndUser endUser)
        {
            if (endUser == null)
                throw new ArgumentNullException(nameof(endUser));

            _context.EndUsers.Remove(endUser);
        }

        public void UpdateEndUser(EndUser endUser)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
