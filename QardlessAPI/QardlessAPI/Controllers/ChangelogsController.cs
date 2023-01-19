using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangelogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChangelogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Changelogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Changelog>>> GetChangelogs()
        {
          if (_context.Changelogs == null)
          return NotFound();

            return await _context.Changelogs.ToListAsync();
        }

        // GET: api/Changelogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Changelog>> GetChangelog(Guid id)
        {
          if (_context.Changelogs == null)
          return NotFound();

            var changelog = await _context.Changelogs.FindAsync(id);

            if (changelog == null)
            return NotFound();

            return changelog;
        }

        // PUT: api/Changelogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChangelog(Guid id, Changelog changelog)
        {
            if (id != changelog.Id)
            return BadRequest();

            _context.Entry(changelog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChangelogExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // POST: api/Changelogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Changelog>> PostChangelog(Changelog changelog)
        {
          if (_context.Changelogs == null)
          return Problem("Entity set 'ApplicationDbContext.Changelogs'  is null.");
          
            _context.Changelogs.Add(changelog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChangelog", new { id = changelog.Id }, changelog);
        }

        // DELETE: api/Changelogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChangelog(Guid id)
        {
            if (_context.Changelogs == null)
            return NotFound();
            
            var changelog = await _context.Changelogs.FindAsync(id);
            if (changelog == null)
            return NotFound();

            _context.Changelogs.Remove(changelog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChangelogExists(Guid id)
        {
            return (_context.Changelogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
