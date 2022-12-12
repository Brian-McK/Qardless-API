using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Qardless.API.Models;
using Qardless.API.Services;

namespace Qardless.API.Controllers
{
    public class ChangelogsController : Controller
    {
        private readonly QardlessAPIContext _context;

        public ChangelogsController(QardlessAPIContext context)
        {
            _context = context;
        }

        // GET: Changelogs
        public async Task<IActionResult> Index()
        {
              return View(await _context.Changelogs.ToListAsync());
        }

        // GET: Changelogs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Changelogs == null)
            {
                return NotFound();
            }

            var changelog = await _context.Changelogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (changelog == null)
            {
                return NotFound();
            }

            return View(changelog);
        }

        // GET: Changelogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Changelogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Content,WasRead,CreatedDate")] Changelog changelog)
        {
            if (ModelState.IsValid)
            {
                changelog.Id = Guid.NewGuid();
                _context.Add(changelog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(changelog);
        }

        // GET: Changelogs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Changelogs == null)
            {
                return NotFound();
            }

            var changelog = await _context.Changelogs.FindAsync(id);
            if (changelog == null)
            {
                return NotFound();
            }
            return View(changelog);
        }

        // POST: Changelogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Type,Content,WasRead,CreatedDate")] Changelog changelog)
        {
            if (id != changelog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(changelog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChangelogExists(changelog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(changelog);
        }

        // GET: Changelogs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Changelogs == null)
            {
                return NotFound();
            }

            var changelog = await _context.Changelogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (changelog == null)
            {
                return NotFound();
            }

            return View(changelog);
        }

        // POST: Changelogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Changelogs == null)
            {
                return Problem("Entity set 'QardlessAPIContext.Changelogs'  is null.");
            }
            var changelog = await _context.Changelogs.FindAsync(id);
            if (changelog != null)
            {
                _context.Changelogs.Remove(changelog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChangelogExists(Guid id)
        {
          return _context.Changelogs.Any(e => e.Id == id);
        }
    }
}
