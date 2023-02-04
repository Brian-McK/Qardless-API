using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Models;
using System.Text;
using System.Security.Cryptography;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IQardlessAPIRepo _qardlessRepo;
        private readonly ApplicationDbContext _context;

        public LoginController(IQardlessAPIRepo qardlessRepo, IMapper mapper, ApplicationDbContext context)
        {
            _qardlessRepo = qardlessRepo ??
                throw new ArgumentNullException(nameof(qardlessRepo));
            _context = context;
        }

        // POST: api/Login/
        [HttpPost()]
        public async Task<ActionResult<Login>> EndUserLogin(EndUserLoginDto loginUser)
        {
            if (loginUser == null)
                return BadRequest();

            EndUser? endUser = _context.EndUsers
                .Where(e => e.Email == loginUser.Email)
                .FirstOrDefault();

            if (endUser == null)
                return NotFound();

            //Security - Hash user passwords
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(loginUser.Password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            var convertedHashedPassword = Convert.ToBase64String(hashedPassword);

            if (endUser.PasswordHash != convertedHashedPassword)
                return Unauthorized();

            endUser.LastLoginDate = DateTime.Now;

            //For frontend
            EndUserReadPartialDto endUserForProps = new EndUserReadPartialDto();
            endUserForProps.Name = endUser.Name;
            endUserForProps.Email = endUser.Email;
            endUserForProps.ContactNumber = endUser.ContactNumber;
            endUserForProps.isLoggedin = true;
            //SendUserDetailsForProps(endUserForProps);

            return Ok();
        }
    }
}
