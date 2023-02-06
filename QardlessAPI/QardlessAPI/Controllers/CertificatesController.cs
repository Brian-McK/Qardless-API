using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Models;
using QardlessAPI.Data.Dtos.Certificate;
using AutoMapper;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context; 
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public CertificatesController(IMapper mapper, ApplicationDbContext context, IQardlessAPIRepo repo)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
            _context = context;
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/Certificates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Certificate>>> GetCertificates()
        {
          if (_context.Certificates == null)
          return NotFound();
         
            return await _context.Certificates.ToListAsync();
        }

        // GET: api/Certificates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Certificate>> GetCertificate(Guid id)
        {
          if (_context.Certificates == null)
          return NotFound();

            var certificate = await _context.Certificates.FindAsync(id);

            if (certificate == null)
            return NotFound();

            return certificate;
        }

        // PUT: api/Certificates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCertificate(Guid id, Certificate certificate)
        {
            if (id != certificate.Id)
            return BadRequest();

            _context.Entry(certificate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificateExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // POST: api/Certificates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Certificate>> PostCertificate(CertificateCreateDto certificateForCreation)
        {
            if (certificateForCreation == null)
                return BadRequest();

            var cert = _mapper.Map<Certificate>(certificateForCreation);

            cert.Id = new Guid();
            cert.CourseTitle = certificateForCreation.CourseTitle;
            cert.CertNumber = certificateForCreation.CertNumber;
            cert.CourseDate = certificateForCreation.CourseDate;
            cert.ExpiryDate = certificateForCreation.ExpiryDate;
            cert.PdfUrl = certificateForCreation.PdfUrl;
            cert.CreatedDate = DateTime.Now;
            cert.EndUserId = certificateForCreation.EndUserId;
            cert.BusinessId = certificateForCreation.BusinessId;

            _repo.PostCertificate(cert);
            _repo.SaveChanges();

            return CreatedAtAction("GetCertificate", new { id = cert.Id }, cert);
        }

        // DELETE: api/Certificates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCertificate(Guid id)
        {
            if (_context.Certificates == null)
            return NotFound();

            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
            return NotFound();
            
            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CertificateExists(Guid id)
        {
            return (_context.Certificates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
