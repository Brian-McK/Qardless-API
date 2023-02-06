using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Models;
using System.Text;
using System.Security.Cryptography;
using AutoMapper;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IQardlessAPIRepo _qardlessRepo;

        public LoginController(IQardlessAPIRepo qardlessRepo)
        {
            _qardlessRepo = qardlessRepo ??
                throw new ArgumentNullException(nameof(qardlessRepo));
        }

        // POST: api/Login/
        [HttpPost()]
        public async Task<ActionResult<EndUser>> EndUserLogin(EndUserLoginDto loginUser)
        {
            if (loginUser == null)
                return BadRequest();

            EndUser? endUser = await _qardlessRepo.GetEndUserByEmail(loginUser);

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
