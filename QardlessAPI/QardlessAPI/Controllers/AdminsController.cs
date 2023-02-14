using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Admins
        [HttpGet("/admins")]
        public async Task<ActionResult<IEnumerable<Admin>>> ViewAllAdmins()
        {
          if (_context.Admins == null)
              return NotFound();
         
            return await _context.Admins.ToListAsync();
        }

        // GET: api/Admins/5
        [HttpGet("/admins/{id}")]
        public async Task<ActionResult<Admin>> GetAdminById(Guid id)
        {
          if (_context.Admins == null)
            return NotFound();
          
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
                return NotFound();

            return admin;
        }

        // PUT: api/Admins/5
        [HttpPut("/admins/{id}")]
        public async Task<IActionResult> EditAdmin(Guid id, Admin admin)
        {
            if (id != admin.Id)
                return BadRequest();

            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Admins
        [HttpPost("/admins")]
        public async Task<ActionResult<Admin>> CreateNewAdmin(Admin admin)
        {
          if (_context.Admins == null)
          return Problem("Entity set 'ApplicationDbContext.Admins'  is null.");

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdminById", new { id = admin.Id }, admin);
        }

        // DELETE: api/Admins/5
        [HttpDelete("/admins/{id}")]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            if (_context.Admins == null)
            return NotFound();

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            return NotFound();

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdminExists(Guid id)
        {
            return (_context.Admins?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
