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
        private readonly ApplicationDbContext _context;
        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Login/
        [HttpPost()]
        public async Task<ActionResult<Login>> GetEndUserLogin(string email, string password)
        {
            //check users have come from db
            if (_context.EndUsers == null)
                return NotFound();

            EndUser endUser = await _context.EndUsers.FirstOrDefaultAsync(e => e.Email == email);

            //check user exists
            if (endUser == null)
                return NotFound();

            //Security - Hash user passwords
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            var convertedHashedPassword = Convert.ToBase64String(hashedPassword);

            if (endUser.PasswordHash != convertedHashedPassword)
                return Unauthorized();

            //TODO: set isLoggedIn to true (default is true atm)
            EndUserLoginDto eu = new EndUserLoginDto();
            eu.isLoggedin = true; 

            return Ok();
        }
    }
}
