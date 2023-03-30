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

        [HttpGet("/endusers")]
        public async Task<ActionResult<EndUser>> AllEndUsers()
        {
            var endUsers = await _repo.ListAllEndUsers();

            if (endUsers == null)
                return NotFound();

            return Ok(endUsers);
        }

        [HttpGet("/endusers/{id}")]
        public async Task<ActionResult<EndUser>> EndUserById(Guid id)
        {
            var endUser = await _repo.GetEndUserById(id);

            if (endUser == null)
                return BadRequest();

            return Ok(_mapper.Map<EndUserReadFullDto>(endUser));
        }

        [HttpGet("/endusers/{id}/certificates")]
        public async Task<ActionResult<Certificate>> ViewEndUsersCertificates(Guid id)
        {
            var endUserCerts = await _repo.GetCertificatesByEndUserId(id);

            if (endUserCerts == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<CertificateReadFullDto>>(endUserCerts));
        }

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
        [HttpPost("/endusers")]
        public async Task<ActionResult<EndUserCreateDto?>> RegisterNewEndUser(EndUserCreateDto endUserForCreation)
        {
            if (endUserForCreation == null)
                return BadRequest();

            EndUserReadPartialDto endUserReadPartialDto = await Task.Run(() => _repo.AddNewEndUser(endUserForCreation));

            return Created("/endusers", endUserReadPartialDto);
        }

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

        [HttpDelete("/endusers/{id}")]
        public async Task<IActionResult> DeleteEndUser(Guid id)
        {
            var endUser = await _repo.GetEndUserById(id);
            if (endUser == null)
                return NotFound();

            _repo.DeleteEndUser(endUser);

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