using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.EndUser;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public LogoutController(IQardlessAPIRepo repo, IMapper mapper)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));
        }

        // PUT: api/Logout
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEndUser(Guid id, EndUserReadPartialDto endUserForLogout)
        {
            if (endUserForLogout == null)
                return BadRequest();

            var endUser = await _repo.GetEndUser(id);
            if (endUser == null)
                return NotFound();

            endUserForLogout.isLoggedin = false;

            _repo.SaveChanges();

            return Ok(endUserForLogout);
        }

    }
}
