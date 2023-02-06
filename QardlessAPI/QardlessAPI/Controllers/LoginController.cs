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
        private readonly IQardlessAPIRepo _repo;

        public LoginController(IQardlessAPIRepo repo)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
        }

        // POST: api/Login/
        [HttpPost()]
        public async Task<ActionResult<EndUser>> EndUserLogin(EndUserLoginDto loginUser)
        {
            if (loginUser == null)
                return BadRequest();

            EndUser? endUser = await _repo.GetEndUserByEmail(loginUser);
            if (endUser == null)
            {
                return Unauthorized();
            }

            endUser.LastLoginDate = DateTime.Now;
            _repo.SaveChanges();

            // @TODO: Use Jacs code to send back a DTO instead of the entire End User object
            //For frontend
            /*
            EndUserReadPartialDto endUserForProps = new EndUserReadPartialDto();
            endUserForProps.Name = endUser.Name;
            endUserForProps.Email = endUser.Email;
            endUserForProps.ContactNumber = endUser.ContactNumber;
            endUserForProps.isLoggedin = true;
            */
            //SendUserDetailsForProps(endUserForProps);

            return Ok(endUser);
        }
    }
}
