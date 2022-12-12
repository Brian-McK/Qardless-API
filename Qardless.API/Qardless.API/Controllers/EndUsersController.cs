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
    public class EndUsersController : Controller
    {
        private readonly QardlessAPIContext _context;

        public EndUsersController(QardlessAPIContext context)
        {
            _context = context;
        }

        // GET: EndUsers
        public async Task<IActionResult> Index()
        {
              return View(await _context.EndUsers.ToListAsync());
        }

        // GET: EndUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.EndUsers == null)
            {
                return NotFound();
            }

            var endUser = await _context.EndUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endUser == null)
            {
                return NotFound();
            }

            return View(endUser);
        }

        // GET: EndUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EndUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Email,EmailVerified,PasswordHash,PhoneMobile,PhoneMobileVerified,PhoneHome,AddressCode,AddressDetailed,CreatedDate,LastLoginDate")] EndUser endUser)
        {
            if (ModelState.IsValid)
            {
                endUser.Id = Guid.NewGuid();
                _context.Add(endUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(endUser);
        }

        // GET: EndUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.EndUsers == null)
            {
                return NotFound();
            }

            var endUser = await _context.EndUsers.FindAsync(id);
            if (endUser == null)
            {
                return NotFound();
            }
            return View(endUser);
        }

        // POST: EndUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,MiddleName,LastName,Email,EmailVerified,PasswordHash,PhoneMobile,PhoneMobileVerified,PhoneHome,AddressCode,AddressDetailed,CreatedDate,LastLoginDate")] EndUser endUser)
        {
            if (id != endUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(endUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EndUserExists(endUser.Id))
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
            return View(endUser);
        }

        // GET: EndUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.EndUsers == null)
            {
                return NotFound();
            }

            var endUser = await _context.EndUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endUser == null)
            {
                return NotFound();
            }

            return View(endUser);
        }

        // POST: EndUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.EndUsers == null)
            {
                return Problem("Entity set 'QardlessAPIContext.EndUsers'  is null.");
            }
            var endUser = await _context.EndUsers.FindAsync(id);
            if (endUser != null)
            {
                _context.EndUsers.Remove(endUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EndUserExists(Guid id)
        {
          return _context.EndUsers.Any(e => e.Id == id);
        }
    }
}
