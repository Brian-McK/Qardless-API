using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Models;
using System.Text;
using System.Security.Cryptography;
using QardlessAPI.Data.Dtos.EndUser;
using AutoMapper;
using QardlessAPI.Data.Dtos.Certificate;

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
            var endUsers = await _repo.GetEndUsers();

            if (endUsers == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<EndUserReadFullDto>>(endUsers));
        }
        
        // GET: api/EndUsers/5
        [HttpGet("/endusers/{id}")]
        public async Task<ActionResult<EndUser>> EndUserById(Guid id)
        {
            var endUser = await _repo.GetEndUser(id);

            if (endUser == null)
                return NotFound();

            return Ok(_mapper.Map<EndUserReadFullDto>(endUser));
        }

        // GET: api/EndUsers/Certificates/5
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
        public async Task<IActionResult> EditEndUser(Guid id, EndUserUpdateDto endUserUpdateDto)
        {
            if (endUserUpdateDto == null)
                return BadRequest();

            var endUser = await _repo.GetEndUser(id);
            if (endUser == null)
                return NotFound();

            _mapper.Map(endUserUpdateDto, endUser);
            _repo.PutEndUser(id, endUser);
            _repo.SaveChanges();

            return Accepted();
        }
        
        // Business logic: Register EndUser
        // POST: api/EndUsers
        [HttpPost("/endusers")]
        public async Task<ActionResult<EndUser>> RegisterNewEndUser(EndUserCreateDto endUserForCreation)
        {
            if(endUserForCreation == null)
                return BadRequest();

            var endUser = _mapper.Map<EndUser>(endUserForCreation);

            #region Security - Hash user passwords
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(endUserForCreation.PasswordHash);
            var hashedPassword = sha.ComputeHash(asByteArray);
            var convertedHashedPassword = Convert.ToBase64String(hashedPassword);
            #endregion

            endUser.Id = new Guid();
            endUser.Name = endUserForCreation.Name;
            endUser.Email = endUserForCreation.Email;
            endUser.EmailVerified = false;
            endUser.PasswordHash = convertedHashedPassword;
            endUser.ContactNumber = endUserForCreation.ContactNumber;
            endUser.CreatedDate = DateTime.Now;
            endUser.LastLoginDate = endUser.CreatedDate;

            _repo.PostEndUser(endUser);
            _repo.SaveChanges();

            return CreatedAtAction("GetEndUser", new { id = endUser.Id }, endUser);
        }

        // Business logic: Logout EndUser
        // POST: api/EndUsers
        [HttpPost("/logout")]
        public async Task<ActionResult<EndUserLogoutDto>> LogoutEndUser(Guid userid)
        {
            var endUser = await _repo.GetEndUser(userid);

            if (endUser == null)
                return BadRequest();

            EndUserLogoutDto endUserForLogout = new EndUserLogoutDto();
            endUserForLogout.Id = userid;
            endUserForLogout.isLoggedin = false;

            return Ok(endUserForLogout);
        }

        // DELETE: api/EndUsers/5
        [HttpDelete("/endusers/{id}")]
        public async Task<IActionResult> DeleteEndUser(Guid id)
        {
            var endUser = await _repo.GetEndUser(id);
            if (endUser == null)
                return NotFound();

            _repo.DeleteEndUser(endUser);
            _repo.SaveChanges();

            return Accepted();
        }

        private bool EndUserExists(Guid id)
        {
            var endUser = _repo.GetEndUser(id);
            if (endUser == null)
                return false;

            return true;
        }
    }
}