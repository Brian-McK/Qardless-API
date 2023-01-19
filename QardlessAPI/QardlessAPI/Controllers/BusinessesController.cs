using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.Business;
using QardlessAPI.Data.Dtos.EndUser;
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
            _repo = repo;
            _mapper = mapper;
        }

        // GET: api/Businesses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Business>>> GetBusinesses()
        {
            var businessItems = await _repo.GetBusinesses();
            if (businessItems == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<BusinessReadPartialDto>>(businessItems));
        }

        // GET: api/Businesses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Business>> GetBusiness(Guid id)
        {
            var business = await _repo.GetBusiness(id);
            if (business == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<BusinessReadFullDto>(business));
        }

        // PUT: api/Businesses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusiness(Guid id, BusinessUpdateDto businessUpdateDto)
        {
            if (businessUpdateDto == null)
            {
                return BadRequest();
            }

            var businessModelFromRepo = await _repo.GetBusiness(id);
            if (businessModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(businessUpdateDto, businessModelFromRepo);
            _repo.PutBusiness(id, businessModelFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }

        // PATCH: api/Businesses/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBusiness(Guid id, JsonPatchDocument<BusinessUpdateDto> patchDoc)
        {
            var businessModelFromRepo = await _repo.GetBusiness(id);

            if (businessModelFromRepo == null)
            {
                return NotFound();
            }

            var businessToPatch = _mapper.Map<BusinessUpdateDto>(businessModelFromRepo);
            patchDoc.ApplyTo(businessToPatch, ModelState);
            
            if(!TryValidateModel(businessToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(businessToPatch, businessModelFromRepo);
            _repo.PatchBusiness(id, businessModelFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }

        // POST: api/Businesses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BusinessReadFullDto>> PostBusiness(BusinessCreateDto businessCreateDto)
        {
            if (businessCreateDto == null)
            {
                return BadRequest();
            }

            var businessModel = _mapper.Map<Business>(businessCreateDto);
            _repo.PostBusiness(businessModel);
            _repo.SaveChanges();

            var businessReadFullDto = _mapper.Map<BusinessReadFullDto>(businessModel);

            return CreatedAtAction(nameof(GetBusiness), new { Id = businessReadFullDto.Id }, businessReadFullDto);
        }

        // DELETE: api/Businesses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusiness(Guid id)
        {
            var businessModelFromRepo = await _repo.GetBusiness(id);
            if (businessModelFromRepo == null)
            {
                return NotFound();
            }

            _repo.DeleteBusiness(businessModelFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }

        private bool BusinessExists(Guid id)
        {
            var businessModelFromRepo = _repo.GetBusiness(id);
            if (businessModelFromRepo == null)
            {
                return false;
            }
            return true;
        }
    }
}
