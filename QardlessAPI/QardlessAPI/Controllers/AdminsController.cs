using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QardlessAPI.Data;
using QardlessAPI.Data.Models;
using QardlessAPI.Data.Dtos.Admin;
using QardlessAPI.Data.Dtos.Authentication;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public AdminsController(IQardlessAPIRepo repo, IMapper mapper)
        {
            _repo = repo ??
                    throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("/admins")]
        public async Task<ActionResult<Admin>> AllAdmins()
        {
            var admins = await _repo.ListAllAdmins();

            if(admins == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<AdminReadDto>>(admins));
        }

        [HttpGet("/admins/{id}")]
        public async Task<ActionResult<Admin>> AdminById(Guid id)
        {
            var admin = await _repo.GetAdminById(id);
            
            if(admin== null) return BadRequest();

            return Ok(_mapper.Map<AdminReadDto>(admin));
        }

        [HttpPut("/admins/{id}")]
        public async Task<ActionResult> UpdateAdminContactDetails(Guid id, AdminUpdateDto adminUpdateDto)
        {
            if(adminUpdateDto == null) return BadRequest();

            var admin = await _repo.GetAdminById(id);
            if(admin == null) return BadRequest();

            await Task.Run(() => _repo.UpdateAdminDetails(id, adminUpdateDto));

            return Accepted(admin);
        }

        // REGISTER
        [HttpPost("/admins")]
        public async Task<ActionResult<AdminCreateDto?>> RegisterNewAdmin(AdminCreateDto newAdmin)
        {
            if(newAdmin == null) return BadRequest();

            AdminPartialDto adminPartialDto = await Task.Run(() => _repo.AddNewAdmin(newAdmin));

            return Created("/admins", adminPartialDto);
        }

        // LOGOUT
        [HttpPost("/admins/logout")]
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

        [HttpDelete("/admins/{id}")]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            var admin = await _repo.GetAdminById(id);
            if (admin == null) return BadRequest();

            _repo.DeleteAdmin(admin);

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
