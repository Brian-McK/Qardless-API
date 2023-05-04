using System.Data;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos;
using QardlessAPI.Data.Models;
using QardlessAPI.Data.Dtos.Authentication;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // TODO: Change to use repo, also add mapper
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtHandler _jwtHandler;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            JwtHandler jwtHandler)
        {
            _context = context;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                return Unauthorized(new LoginResult()
                {
                    Success = false,
                    Message = "Invalid Email or Password."
                });
            }

            var secToken = await _jwtHandler.GetTokenAsync(user);
            var jwt = new JwtSecurityTokenHandler().WriteToken(secToken);
            return Ok(new LoginResult()
            {
                Success = true,
                Message = "Login successful",
                Token = jwt
            });
        }

        [HttpGet]
        [Authorize] // Only allows access to Authenticated users.
        public async Task<ActionResult> GetAccountInfo()
        {
            var user = await _userManager.FindByNameAsync(
                HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value);

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }
        // TODO, have GET for all Accounts
        /*
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<Action>
        */
    }
}