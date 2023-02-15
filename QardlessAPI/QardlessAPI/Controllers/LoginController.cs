using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Dtos.Authentication;
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

        // POST: api/login/
        [HttpPost("/endusers/login")]
        public async Task<ActionResult<EndUserReadPartialDto>> LoginEndUser(LoginDto loginUser)
        {
            EndUser? endUser = await _repo.GetEndUserByEmail(loginUser);

            if (loginUser == null || endUser == null)
                return BadRequest();

            if (!_repo.CheckEndUserPassword(endUser, loginUser))
                return Unauthorized();
            
            EndUserReadPartialDto endUserForProps = _repo.SendEndUserForProps(endUser);
            return Ok(endUserForProps);
        }
    }
}
