using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QardlessAPI.Data;
using QardlessAPI.Data.Models;
using QardlessAPI.Data.Services;
using QardlessAPI.Data.Dtos.Admin;
using QardlessAPI.Data.Dtos.Authentication;
using Microsoft.AspNetCore.Authorization;

using System.Data;
using Microsoft.EntityFrameworkCore;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Note: having Authorize here causes issues with /api 
    public class AdminsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public AdminsController(
            IQardlessAPIRepo repo,
            IMapper mapper,
            ApplicationDbContext context
            )
        {
            _repo = repo ??
                    throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));

            _context = context;
        }
        // Email
        /*
        [HttpPost("/admins/resetpassword")]
        public async void ResetPassword()
        {
            EmailSender emailSender;

            var resetLink = "test";

            var email = "finbar.xp@gmail.com";
            var subject = "Reset Password for Qardless";
            var message = $"Please reset your password for Qardless using this link: {resetLink}";

            await emailSender.SendEmailAsync(email, subject, message);
        }
        */


        // GET: api/Admins
        [HttpGet]//("/admins")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<Admin>>> AllAdmins()
        {
            var admins = await _repo.ListAllAdmins();
            //var admins = await _context.Admins.ToListAsync();

            if (admins == null)
                return NotFound();

            return Ok(admins);
            //return Ok(_mapper.Map<IEnumerable<AdminReadDto>>(admins));
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Admin>> AdminById(Guid id)
        {
            var admin = await _repo.GetAdminById(id);
            
            if(admin== null) return BadRequest();

            return Ok(_mapper.Map<AdminReadDto>(admin));
        }

        // PUT: api/Admins/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> UpdateAdminContactDetails(Guid id, AdminUpdateDto adminUpdateDto)
        {
            if(adminUpdateDto == null) return BadRequest();

            var admin = await _repo.GetAdminById(id);
            if(admin == null) return BadRequest();

            await Task.Run(() => _repo.UpdateAdminDetails(id, adminUpdateDto));

            return Accepted(admin);
        }

        // POST: api/Admins    (REGISTER)
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<AdminCreateDto?>> RegisterNewAdmin(AdminCreateDto newAdmin)
        {
            if(newAdmin == null) return BadRequest();

            AdminReadDto adminReadDto = await Task.Run(() => _repo.AddNewAdmin(newAdmin));

            return Created("/admins", adminReadDto);
        }

        //LOGOUT
        // POST: api/admins/logout
        // Should be in the Account Controller
        [HttpPost("logout")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<LogoutResponseDto>> LogoutAdmin(
            [FromBody] LogoutRequestDto adminLogoutRequest)
        {
            var admin = await _repo.GetAdminById(adminLogoutRequest.Id);
            if (admin == null) return BadRequest();

            LogoutResponseDto adminLogoutResponse = new LogoutResponseDto
            {
                Id = adminLogoutRequest.Id,
                IsLoggedIn = false
            };

            return Ok(adminLogoutResponse);
        }

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            var admin = await _repo.GetAdminById(id);
            if (admin == null) return BadRequest();

            _repo.DeleteAdmin(admin);
            _repo.SaveChanges();

            return Accepted();
        }

        private bool CheckAdminExists(Guid id)
        {
            var admin = _repo.GetAdminById(id);
            if (admin == null) 
                return false;

            return true;
        }
    }
}
