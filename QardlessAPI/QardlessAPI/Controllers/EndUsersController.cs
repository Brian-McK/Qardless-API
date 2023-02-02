using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Models;
using System.Text;
using System.Security.Cryptography;
using QardlessAPI.Data.Dtos.EndUser;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EndUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EndUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EndUser>>> GetEndUsers()
        {
          if (_context.EndUsers == null)
          return NotFound();

            return await _context.EndUsers.ToListAsync();
        }

        // GET: api/EndUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EndUser>> GetEndUser(Guid id)
        {
          if (_context.EndUsers == null)
          return NotFound();

            var endUser = await _context.EndUsers.FindAsync(id);

            if (endUser == null)
            return NotFound();

            return endUser;
        }

        // PUT: api/EndUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEndUser(Guid id, EndUser endUser)
        {
            if (id != endUser.Id)
            return BadRequest();

            _context.Entry(endUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EndUserExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Business logic: Register EndUser
        // POST: api/EndUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost()]
        public async Task<ActionResult<EndUser>> PostEndUser(EndUserCreateDto endUserForCreation)
        {
            if (_context.EndUsers == null)
                return NotFound();

            //Security - Hash user passwords
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(endUserForCreation.PasswordHash);
            var hashedPassword = sha.ComputeHash(asByteArray);
            var convertedHashedPassword = Convert.ToBase64String(hashedPassword);

            EndUser endUser = new EndUser();

            endUser.Id = new Guid();
            endUser.Name = endUserForCreation.Name;
            endUser.Email = endUserForCreation.Email;
            endUser.EmailVerified = false;
            endUser.PasswordHash = convertedHashedPassword;
            endUser.ContactNumber = endUserForCreation.ContactNumber;
            endUser.CreatedDate = DateTime.Now;
            endUser.LastLoginDate = endUser.CreatedDate;

            _context.EndUsers.Add(endUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEndUser", new { id = endUser.Id }, endUser);
        }

        // DELETE: api/EndUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEndUser(Guid id)
        {
            if (_context.EndUsers == null)
            return NotFound();

            var endUser = await _context.EndUsers.FindAsync(id);
            if (endUser == null)
            return NotFound();

            _context.EndUsers.Remove(endUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EndUserExists(Guid id)
        {
            return (_context.EndUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
