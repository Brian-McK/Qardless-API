using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.Certificate;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Dtos.Authentication;
using QardlessAPI.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EndUsersController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtHandler _jwtHandler;

        public EndUsersController(
            IQardlessAPIRepo repo,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            JwtHandler jwtHandler)
        {
            _repo = repo ??
                    throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));

            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        // GET: api/EndUsers
        [HttpGet]
        //[Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<EndUser>>> AllEndUsers()
        {
            var endUsers = await _repo.ListAllEndUsers();

            if (endUsers == null)
                return NotFound();

            return Ok(endUsers);
        }

        // GET: api/EndUsers/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<EndUser>> EndUserById(Guid id)
        {
            var endUser = await _repo.GetEndUserById(id);

            if (endUser == null)
                return BadRequest();

            return Ok(_mapper.Map<EndUserReadFullDto>(endUser));
        }

        // GET: api/EndUsers/5/Certificates/
        [HttpGet("{id}/certificates")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Certificate>> ViewEndUsersCertificates(Guid id)
        {
            var endUserCerts = await _repo.GetCertificatesByEndUserId(id);

            if (endUserCerts == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<CertificateReadFullDto>>(endUserCerts));
        }

        // PUT: api/EndUsers/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
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
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<EndUserCreateDto?>> RegisterNewEndUser(EndUserCreateDto endUserForCreation)
        {
            if (endUserForCreation == null)
                return BadRequest();

            EndUserReadFullDto endUserReadFullDto = await Task.Run(() => _repo.AddNewEndUser(endUserForCreation));

            return Created("/endusers", endUserReadFullDto);
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
        [HttpPut("certificates/unassign/{id}")]
        [Authorize(Roles = "Administrator, RegisteredUser")]
        public async Task<ActionResult> UnassignCertFromEndUser(CertificateReadPartialDto cert)
        {
            if(cert== null)
                return BadRequest();

            await Task.Run(() => _repo.UnassignCert(cert));

            return Ok();
        }


        // Business logic: Logout EndUser
        // POST: api/EndUsers
        [HttpPost("logout")]
        [Authorize(Roles = "RegisteredUser")]
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
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
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