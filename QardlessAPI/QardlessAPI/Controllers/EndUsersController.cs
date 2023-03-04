using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.Certificate;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Dtos.Authentication;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndUsersController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public EndUsersController(IQardlessAPIRepo repo, IMapper mapper)
        {
            _repo = repo ??
                    throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/EndUsers
        [HttpGet("/endusers")]
        public async Task<ActionResult<EndUser>> AllEndUsers()
        {
            var endUsers = await _repo.ListAllEndUsers();

            if (endUsers == null)
                return NotFound();

            return Ok(endUsers);
        }

        // GET: api/EndUsers/5
        [HttpGet("/endusers/{id}")]
        public async Task<ActionResult<EndUser>> EndUserById(Guid id)
        {
            var endUser = await _repo.GetEndUserById(id);

            if (endUser == null)
                return BadRequest();

            return Ok(_mapper.Map<EndUserReadFullDto>(endUser));
        }

        // GET: api/EndUsers/5/Certificates/
        [HttpGet("/endusers/{id}/certificates")]
        public async Task<ActionResult<Certificate>> ViewEndUsersCertificates(Guid id)
        {
            var endUserCerts = await _repo.GetCertificatesByEndUserId(id);

            if (endUserCerts == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<CertificateReadFullDto>>(endUserCerts));
        }

        // PUT: api/EndUsers/5
        [HttpPut("/endusers/{id}")]
        public async Task<ActionResult> UpdateEndUserContactDetails(Guid id, EndUserUpdateDto endUserUpdateDto)
        {
            if (endUserUpdateDto == null)
                return BadRequest();

            var endUser = await _repo.GetEndUserById(id);
            if (endUser == null)
                return BadRequest();

            await Task.Run(() => _repo.UpdateEndUserDetails(id, endUserUpdateDto));

            return Accepted(endUser);
        }

        // Business logic: Register EndUser
        // POST: api/EndUsers
        [HttpPost("/endusers")]
        public async Task<ActionResult<EndUserCreateDto?>> RegisterNewEndUser(EndUserCreateDto endUserForCreation)
        {
            if (endUserForCreation == null)
                return BadRequest();

            EndUserReadPartialDto endUserReadPartialDto = await Task.Run(() => _repo.AddNewEndUser(endUserForCreation));

            return Created("/endusers", endUserReadPartialDto);
        }

        /*// Business logic: Assign cert to enduser
        // PUT : api/enduser/7/cert
        [HttpPut("enduser/{id}/certificates")]
        public async Task<ActionResult> AssignCertToEndUser(CertToAssignDto certToAssign)
        {
            if(certToAssign == null)
                return BadRequest();

            await Task.Run(() => _repo.AssignCert(certToAssign));

            return Ok();
        }*/


        // WEB APP - UNASSIGN CERT
        [HttpPut("/endusers/certificates/unassign/{id}")]
        public async Task<ActionResult> UnassignCertFromEndUser(CertificateReadPartialDto cert)
        {
            if(cert== null)
                return BadRequest();

            await Task.Run(() => _repo.UnassignCert(cert));

            return Ok();
        }

        // Business logic: Logout EndUser
        // POST: api/EndUsers
        [HttpPost("/endusers/logout")]
        public async Task<ActionResult<LogoutResponseDto>> LogoutEndUser(
            [FromBody] LogoutRequestDto endUserLogoutRequest)
        {
            var endUser = await _repo.GetEndUserById(endUserLogoutRequest.Id);

            if (endUser == null)
                return BadRequest();

            LogoutResponseDto endUserLogoutResponse = new LogoutResponseDto
            {
                Id = endUserLogoutRequest.Id,
                IsLoggedIn = false
            };

            return Ok(endUserLogoutResponse);
        }

        // DELETE: api/EndUsers/5
        [HttpDelete("/endusers/{id}")]
        public async Task<IActionResult> DeleteEndUser(Guid id)
        {
            var endUser = await _repo.GetEndUserById(id);
            if (endUser == null)
                return NotFound();

            _repo.DeleteEndUser(endUser);
            _repo.SaveChanges();

            return Accepted();
        }

        private bool CheckEndUserExists(Guid id)
        {
            var endUser = _repo.GetEndUserById(id);
            if (endUser == null)
                return false;

            return true;
        }
    }
}