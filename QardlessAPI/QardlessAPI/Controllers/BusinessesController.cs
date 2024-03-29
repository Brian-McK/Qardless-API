﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.Authentication;
using QardlessAPI.Data.Dtos.Business;
using QardlessAPI.Data.Dtos.Employee;
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

        [HttpGet("/businesses")]
        public async Task<ActionResult<IEnumerable<Business>>> ViewAllBusinesses()
        {
            var businesses = await _repo.ListAllBusinesses();
            
            if (businesses == null) return NotFound();

            return Ok(_mapper.Map<IEnumerable<BusinessReadPartialDto>>(businesses));
        }

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

        [HttpPost("/businesses")]
        public async Task<ActionResult<BusinessCreateDto?>> RegisterNewBusiness(BusinessCreateDto businessCreateDto)
        {
            if (businessCreateDto == null)
                return BadRequest();

            LoginDto businessCheck = new LoginDto
            {
                Email = businessCreateDto.Email
            };

            if (_repo.GetBusinessByEmail(businessCheck).Result == null)
            {
                BusinessReadPartialDto businessReadPartialDto = await Task.Run(() => _repo.AddNewBusiness(businessCreateDto));

                return Created("/businesses", businessReadPartialDto);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("/businesses/{id}")]
        public async Task<IActionResult> DeleteBusiness(Guid id)
        {
            var business = await _repo.GetBusinessById(id);
            if (business == null)
                return NotFound();

            _repo.DeleteBusiness(business);

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
