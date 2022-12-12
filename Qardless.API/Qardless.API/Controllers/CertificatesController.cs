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
    public class CertificatesController : Controller
    {
        private readonly QardlessAPIContext _context;

        public CertificatesController(QardlessAPIContext context)
        {
            _context = context;
        }

        // GET: Certificates
        public async Task<IActionResult> Index()
        {
            var qardlessAPIContext = _context.Certificates.Include(c => c.Business).Include(c => c.EndUser);
            return View(await qardlessAPIContext.ToListAsync());
        }

        // GET: Certificates/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Certificates == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates
                .Include(c => c.Business)
                .Include(c => c.EndUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }

        // GET: Certificates/Create
        public IActionResult Create()
        {
            ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "Email");
            ViewData["EndUserId"] = new SelectList(_context.EndUsers, "Id", "AddressCode");
            return View();
        }

        // POST: Certificates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,QrCodeUri,PdfUri,SerialNumber,Expires,ExpiryDate,CreatdeDate,EndUserId,BusinessId")] Certificate certificate)
        {
            if (ModelState.IsValid)
            {
                certificate.Id = Guid.NewGuid();
                _context.Add(certificate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "Email", certificate.BusinessId);
            ViewData["EndUserId"] = new SelectList(_context.EndUsers, "Id", "AddressCode", certificate.EndUserId);
            return View(certificate);
        }

        // GET: Certificates/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Certificates == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return NotFound();
            }
            ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "Email", certificate.BusinessId);
            ViewData["EndUserId"] = new SelectList(_context.EndUsers, "Id", "AddressCode", certificate.EndUserId);
            return View(certificate);
        }

        // POST: Certificates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,QrCodeUri,PdfUri,SerialNumber,Expires,ExpiryDate,CreatdeDate,EndUserId,BusinessId")] Certificate certificate)
        {
            if (id != certificate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(certificate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CertificateExists(certificate.Id))
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
            ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "Email", certificate.BusinessId);
            ViewData["EndUserId"] = new SelectList(_context.EndUsers, "Id", "AddressCode", certificate.EndUserId);
            return View(certificate);
        }

        // GET: Certificates/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Certificates == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates
                .Include(c => c.Business)
                .Include(c => c.EndUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }

        // POST: Certificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Certificates == null)
            {
                return Problem("Entity set 'QardlessAPIContext.Certificates'  is null.");
            }
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate != null)
            {
                _context.Certificates.Remove(certificate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CertificateExists(Guid id)
        {
          return _context.Certificates.Any(e => e.Id == id);
        }
    }
}
