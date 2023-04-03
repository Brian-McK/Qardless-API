using Microsoft.AspNetCore.Mvc;
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
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public CertificatesController(IMapper mapper, IQardlessAPIRepo repo)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("/certificates")]
        public async Task<ActionResult<Certificate>> ViewAllCertificates()
        {
            var certs = await _repo.ListAllCertificates();
            
            if (certs == null)
                return NotFound();

            return Ok(certs);
        }

        [HttpGet("/certificates/{id}")]
        public async Task<ActionResult<Certificate>> CertificateById(Guid id)
        {
            var cert = await _repo.GetCertificateById(id);

            if (cert == null)
                return NotFound();

            return Ok(_mapper.Map<CertificateReadFullDto>(cert));
        }

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

        [HttpPost("/certificates")]
        public async Task<ActionResult<Certificate?>> AddNewCertificate(CertificateCreateDto certificateForCreation)
        {
            if (certificateForCreation == null)
                return BadRequest();

            if (_repo.FindCertificateByCertNumber(certificateForCreation.CertNumber).Result == null)
            {
                var cert = await Task.Run(() => _repo.AddNewCertificate(certificateForCreation));

                return Created("/certificates", cert);
            }
            else
            {
                return BadRequest();
            }
        }

        // WEB APP - FREEZE CERT
        [HttpPut("/certificates/{id}/freeze")]
        public async Task<ActionResult> FreezeEndUserCert(Guid id)
        {
            var cert = await _repo.GetCertificateById(id);

            if(cert == null) return NotFound();

            _repo.FreezeCertificate(cert);

            return Ok();
        }

        // WEB APP - UNFREEZE CERT
        [HttpPut("/certificates/{id}/unfreeze")]
        public async Task<ActionResult> UnfreezeEndUserCert(Guid id)
        {
            var cert = await _repo.GetCertificateById(id);

            if (cert == null) return NotFound();

            _repo.UnfreezeCertificate(cert);

            return Ok();
        }

        [HttpDelete("/certificates/{id}")]
        public async Task<IActionResult> DeleteCertificate(Guid id)
        {
            var cert = await _repo.GetCertificateById(id);
            
            if(cert == null)
                return NotFound();

            _repo.DeleteCertificate(cert);

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
