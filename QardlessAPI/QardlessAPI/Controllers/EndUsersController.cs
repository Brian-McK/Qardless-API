using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Models;
using System.Text;
using System.Security.Cryptography;
using QardlessAPI.Data.Dtos.EndUser;
using AutoMapper;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndUsersController : ControllerBase
    {
        private readonly IQardlessAPIRepo _qardlessRepo;
        private readonly IMapper _mapper;

        public EndUsersController(IQardlessAPIRepo qardlessRepo, IMapper mapper)
        {
            _qardlessRepo = qardlessRepo ??
                throw new ArgumentNullException(nameof(qardlessRepo));
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/EndUsers
        [HttpGet]
        public async Task<ActionResult<EndUser>> GetEndUsers()
        {
            var endUsers = await _qardlessRepo.GetEndUsers();

            if (endUsers == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<EndUserReadFullDto>>(endUsers));
        }
        
        // GET: api/EndUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EndUser>> GetEndUser(Guid id)
        {
            var endUser = await _qardlessRepo.GetEndUser(id);

            if (endUser == null)
                return NotFound();

            return Ok(_mapper.Map<EndUserReadFullDto>(endUser));
        }

        // PUT: api/EndUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEndUser(Guid id, EndUserUpdateDto endUserUpdateDto)
        {
            if (endUserUpdateDto == null)
                return BadRequest();

            var endUser = await _qardlessRepo.GetEndUser(id);
            if (endUser == null)
                return NotFound();

            _mapper.Map(endUserUpdateDto, endUser);
            _qardlessRepo.PutEndUser(id, endUser);
            _qardlessRepo.SaveChanges();

            return Accepted();
        }
        
        // Business logic: Register EndUser
        // POST: api/EndUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost()]
        public async Task<ActionResult<EndUser>> PostEndUser(EndUserCreateDto endUserForCreation)
        {
            if(endUserForCreation == null)
                return BadRequest();

            var endUser = _mapper.Map<EndUser>(endUserForCreation);

            //Security - Hash user passwords
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(endUserForCreation.PasswordHash);
            var hashedPassword = sha.ComputeHash(asByteArray);
            var convertedHashedPassword = Convert.ToBase64String(hashedPassword);

            endUser.Id = new Guid();
            endUser.Name = endUserForCreation.Name;
            endUser.Email = endUserForCreation.Email;
            endUser.EmailVerified = false;
            endUser.PasswordHash = convertedHashedPassword;
            endUser.ContactNumber = endUserForCreation.ContactNumber;
            endUser.CreatedDate = DateTime.Now;
            endUser.LastLoginDate = endUser.CreatedDate;

            _qardlessRepo.PostEndUser(endUser);
            _qardlessRepo.SaveChanges();

            return CreatedAtAction("GetEndUser", new { id = endUser.Id }, endUser);
        }
        
        // DELETE: api/EndUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEndUser(Guid id)
        {
            var endUser = await _qardlessRepo.GetEndUser(id);
            if (endUser == null)
                return NotFound();

            _qardlessRepo.DeleteEndUser(endUser);
            _qardlessRepo.SaveChanges();

            return Accepted();
        }

        private bool EndUserExists(Guid id)
        {
            var endUser = _qardlessRepo.GetEndUser(id);
            if (endUser == null)
                return false;

            return true;
        }
    }
}