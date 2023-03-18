using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.Business;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessesController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public BusinessesController(IQardlessAPIRepo repo, IMapper mapper)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/Businesses
        [HttpGet("/businesses")]
        public async Task<ActionResult<IEnumerable<Business>>> ViewAllBusinesses()
        {
            var businesses = await _repo.ListAllBusinesses();
            
            if (businesses == null) return NotFound();

            return Ok(_mapper.Map<IEnumerable<BusinessReadPartialDto>>(businesses));
        }

        // GET: api/Businesses/5
        [HttpGet("/businesses/{id}")]
        public async Task<ActionResult<Business>> BusinessById(Guid id)
        {
            var business = await _repo.GetBusinessById(id);
            
            if (business == null) return BadRequest();
            
            return Ok(_mapper.Map<BusinessReadFullDto>(business));
        }

        [HttpGet("/businesses/{id}/certificates")]
        public async Task<ActionResult<Certificate>> ViewBusinessesCertificates(Guid id)
        {
            var businessCerts = await _repo.GetCertificateByBusinessId(id);

            if (businessCerts == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<Certificate>>(businessCerts));
        }

        // PUT: api/Businesses/5
        [HttpPut("/businesses/{id}")]
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
        [HttpPost("/businesses")]
        public async Task<ActionResult<BusinessCreateDto?>> RegisterNewBusiness(BusinessCreateDto businessCreateDto)
        {
            if (businessCreateDto == null)
                return BadRequest();

            BusinessReadPartialDto businessReadPartialDto = await Task.Run(() => _repo.AddNewBusiness(businessCreateDto));

            return Created("/businesses", businessReadPartialDto);
        }

        // DELETE: api/Businesses/5
        [HttpDelete("/businesses/{id}")]
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
