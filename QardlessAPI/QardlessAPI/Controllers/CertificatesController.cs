using Microsoft.AspNetCore.Mvc;
using QardlessAPI.Data;
using QardlessAPI.Data.Models;
using QardlessAPI.Data.Dtos.Certificate;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [HttpGet]
        public async Task<ActionResult<Certificate>> ViewAllCertificates()
        {
            var certs = await _repo.ListAllCertificates();
            
            if (certs == null)
                return NotFound();

            return Ok(certs);
        }

        // GET: api/Certificates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Certificate>> CertificateById(Guid id)
        {
            var cert = await _repo.GetCertificateById(id);

            if (cert == null)
                return NotFound();

            return Ok(_mapper.Map<CertificateReadFullDto>(cert));
        }

        // PUT: api/Certificates/5
        [HttpPut("{id}")]
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
        [HttpPost]
        public async Task<ActionResult<Certificate?>> AddNewCertificate(CertificateCreateDto certificateForCreation)
        {
            if (certificateForCreation == null)
                return BadRequest();
            
            var cert = _repo.AddNewCertificate(certificateForCreation);

            if (cert == null) return BadRequest();

            return Created("/certificates", cert);
        }

        #region WEB APP - BUSINESS LOGIC - ASSIGN CERT TO ENDUSER (NOT IMPLEMENTED HERE)
        // Commented out the below code as we dont need a controller to assign a cert. The assign a cert is
        // done in the create a certificate. We then populate the certificate list in the end user object. Kept this
        // commented if we need still need it.

        /* 
        // PUT : api/enduser/7/cert
        [HttpPut("enduser/{id}/certificates")]
        public async Task<ActionResult> AssignCertToEndUser(CertToAssignDto certToAssign)
        {
            if (certToAssign == null)
                return BadRequest();

            await Task.Run(() => _repo.AssignCert(certToAssign));

            return Ok();
        }*/
        #endregion

        // WEB APP - FREEZE CERT
        // PUT: api/certificates/freeze/5
        [HttpPut("{id}/freeze")]
        public async Task<ActionResult> FreezeEndUserCert(Guid id)
        {
            var cert = await _repo.GetCertificateById(id);

            if(cert == null) return NotFound();

            _repo.FreezeCertificate(cert);

            return Ok();
        }

        // WEB APP - UNFREEZE CERT
        // PUT: api/certificates/unfreeze/5
        [HttpPut("{id}/unfreeze")]
        public async Task<ActionResult> UnfreezeEndUserCert(Guid id)
        {
            var cert = await _repo.GetCertificateById(id);

            if (cert == null) return NotFound();

            _repo.UnfreezeCertificate(cert);

            return Ok();
        }

        // DELETE: api/Certificates/5
        [HttpDelete("{id}")]
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
