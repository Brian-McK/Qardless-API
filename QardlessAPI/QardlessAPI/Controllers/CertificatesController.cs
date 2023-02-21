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
using QardlessAPI.Data.Dtos.EndUser;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public CertificatesController(IMapper mapper, IQardlessAPIRepo repo)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/Certificates
        [HttpGet("/certificates")]
        public async Task<ActionResult<Certificate>> ViewAllCertificates()
        {
            var certs = await _repo.ListAllCertificates();
            
            if (certs == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<CertificateReadFullDto>>(certs));
        }

        // GET: api/Certificates/5
        [HttpGet("/certificates/{id}")]
        public async Task<ActionResult<Certificate>> CertificateById(Guid id)
        {
            var cert = await _repo.GetCertificateById(id);

            if (cert == null)
                return NotFound();

            return Ok(_mapper.Map<CertificateReadFullDto>(cert));
        }

        // PUT: api/Certificates/5
        [HttpPut("/certificates/{id}")]
        public async Task<IActionResult> EditCertificate(Guid id, CertificateUpdateDto? certForUpdateDto)
        {
            if(certForUpdateDto == null)
                return NotFound();

            var cert = await _repo.GetCertificateById(id);
            if (cert == null)
                return NotFound();

            await Task.Run(() => _repo.UpdateCertificate(id, certForUpdateDto));

            return Accepted(cert);
        }

        // POST: api/Certificates
        [HttpPost("/certificates")]
        public async Task<ActionResult<Certificate>> AddNewCertificate(CertificateCreateDto certificateForCreation)
        {
            if (certificateForCreation == null)
                return BadRequest();

            var cert = await Task.Run(() => _mapper.Map<Certificate>(certificateForCreation));

            cert.Id = new Guid();
            cert.CourseTitle = certificateForCreation.CourseTitle;
            cert.CertNumber = certificateForCreation.CertNumber;
            cert.CourseDate = certificateForCreation.CourseDate;
            cert.ExpiryDate = certificateForCreation.ExpiryDate;
            cert.PdfUrl = certificateForCreation.PdfUrl;
            cert.CreatedDate = DateTime.Now;
            cert.EndUserId = certificateForCreation.EndUserId;
            cert.BusinessId = certificateForCreation.BusinessId;

            _repo.AddNewCertificate(cert);
            _repo.SaveChanges();

            return CreatedAtAction("CertificateById", new { id = cert.Id }, cert);
        }

        // DELETE: api/Certificates/5
        [HttpDelete("/certificates/{id}")]
        public async Task<IActionResult> DeleteCertificate(Guid id)
        {
            var cert = await _repo.GetCertificateById(id);
            
            if(cert == null)
                return NotFound();

            _repo.DeleteCertificate(cert);
            _repo.SaveChanges();

            return Accepted();
        }

        private bool CheckCertificateExists(Guid id)
        {
            var cert = _repo.GetCertificateById(id);
            if (cert == null)
                return false;

            return true;
        }
    }
}
