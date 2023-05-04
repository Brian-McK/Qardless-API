using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.Business;
using QardlessAPI.Data.Models;
using System.Data;
using System.Security.Claims;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessesController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;
        // TODO: Move DI below to repo
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtHandler _jwtHandler;

        public BusinessesController(
            IQardlessAPIRepo repo,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            JwtHandler jwtHandler)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));

            _userManager = userManager;
            _jwtHandler= jwtHandler;
        }

        // GET: api/Businesses
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<Business>>> ViewAllBusinesses()
        {
            var businesses = await _repo.ListAllBusinesses();
            
            if (businesses == null) return NotFound();

            return Ok(_mapper.Map<IEnumerable<BusinessReadPartialDto>>(businesses));
        }

        // GET: api/Businesses/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Business>> BusinessById(Guid id)
        {
            var business = await _repo.GetBusinessById(id);
            
            if (business == null) return BadRequest();
            
            return Ok(_mapper.Map<BusinessReadFullDto>(business));
        }


        [HttpGet("{id}/certificates")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Certificate>> ViewBusinessesCertificates(Guid id)
        {
            var businessCerts = await _repo.GetCertificateByBusinessId(id);

            if (businessCerts == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<Certificate>>(businessCerts));
        }

        [HttpGet("certificates")]
        [Authorize(Roles = "Business")]
        public async Task<ActionResult<Certificate>> ViewBusinessesCertificates()
        {
            var user = await _userManager.FindByNameAsync(
                HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value);

            if (user == null)
            {
                return BadRequest();
            }


            var businessCerts = await _repo.GetCertificateByBusinessId(new Guid(user.Id));

            if (businessCerts == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<Certificate>>(businessCerts));
        }


        [HttpGet("{id}/exp/certificates")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Certificate>> ViewCertsDueForRenewalByBusiness(Guid id)
        {
            var certs = await _repo.GetCertsDueForRenewal(id);

            if (certs == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<Certificate>>(certs));
        }

        [HttpGet("exp/certificates")]
        [Authorize(Roles = "Business")]
        public async Task<ActionResult<Certificate>> ViewCertsDueForRenewalByBusinessJWT()
        {
            var user = await _userManager.FindByNameAsync(
                HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value);

            if (user == null)
            {
                return BadRequest();
            }

            var certs = await _repo.GetCertsDueForRenewal(new Guid(user.Id));

            if (certs == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<Certificate>>(certs));
        }

        // PUT: api/Businesses/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> UpdateBusinessContactDetails(Guid id, BusinessUpdateDto businessUpdateDto)
        {
            if (businessUpdateDto == null)
                return BadRequest();

            var business = await _repo.GetBusinessById(id);
            if (business == null)
                return BadRequest();

            await Task.Run(() => _repo.UpdateBusinessDetails(id, businessUpdateDto));

            return Accepted(business);
        }

        // POST: api/Businesses
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<BusinessCreateDto?>> RegisterNewBusiness(BusinessCreateDto businessCreateDto)
        {
            if (businessCreateDto == null)
                return BadRequest();

            BusinessReadFullDto businessReadFullDto = await Task.Run(() => _repo.AddNewBusiness(businessCreateDto));

            return Created("/businesses", businessReadFullDto);
        }

        // DELETE: api/Businesses/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteBusiness(Guid id)
        {
            var business = await _repo.GetBusinessById(id);
            if (business == null)
                return NotFound();

            _repo.DeleteBusiness(business);
            _repo.SaveChanges();

            return Accepted();
        }

        private bool CheckBusinessExists(Guid id)
        {
            var business = _repo.GetBusinessById(id);
            if (business == null)
                return false;
            
            return true;
        }
    }
}
