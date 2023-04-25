using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.FlaggedIssue;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlaggedIssueController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public FlaggedIssueController(IQardlessAPIRepo repo, IMapper mapper)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("/flaggedissues")]
        public async Task<ActionResult<FlaggedIssue>> ViewAllFlaggedIssues()
        {
            var flaggedIssues = await _repo.ListAllFlaggedIssues();

            if (flaggedIssues == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<FlaggedIssueReadDto>>(flaggedIssues));
        }

        [HttpGet("/flaggedissues/{id}")]
        public async Task<ActionResult<FlaggedIssue>> FlaggedIssueById(Guid id)
        {
            var flaggedIssue = await _repo.GetFlaggedIssueById(id);

            if (flaggedIssue == null)
                return NotFound();

            return Ok(_mapper.Map<FlaggedIssueReadDto>(flaggedIssue));
        }

        [HttpGet("/flaggedissues/businesses/{id}")]
        public async Task<ActionResult<FlaggedIssue>> ViewFlaggedIssuesByBusiness(Guid id)
        {
            var certs = await _repo.ListFlaggedIssuesByBusinessId(id);

            if(certs == null)
                return NotFound();

            return Ok(certs);
        }

        [HttpPut("/flaggedissues/{id}")]
        public async Task<ActionResult> UpdateFlaggedIssue(Guid id, FlaggedIssueUpdateDto flaggedIssueDto)
        {
            if (flaggedIssueDto == null)
                return BadRequest();

            var flaggedIssue = await _repo.GetFlaggedIssueById(id);
            if (flaggedIssue == null)
                return BadRequest();

            await Task.Run(() => _repo.UpdateFlaggedIssueWasRead(id, flaggedIssueDto));

            return Accepted(flaggedIssue);
        }

        [HttpPost("/flaggedissues")]
        public async Task<ActionResult<FlaggedIssue>> AddNewFlaggedIssue(FlaggedIssueCreateDto flaggedIssueForCreation)
        {
            if(flaggedIssueForCreation == null)
                return BadRequest();

            var flaggedIssue = _mapper.Map<FlaggedIssue>(flaggedIssueForCreation);

            flaggedIssue.Id = Guid.NewGuid();
            flaggedIssue.Type = flaggedIssueForCreation.Type;
            flaggedIssue.Content = flaggedIssueForCreation.Content;
            flaggedIssue.WasRead = false;
            flaggedIssue.CreatedAt = DateTime.Now;

            _repo.PostFlaggedIssue(flaggedIssue);
            _repo.SaveChanges();

            return CreatedAtAction("FlaggedIssueById", new { id = flaggedIssue.Id }, flaggedIssue);
        }

        [HttpDelete("/flaggedissues/{id}")]
        public async Task<IActionResult> DeleteFlaggedIssue(Guid id)
        {
            var flaggedIssue = await _repo.GetFlaggedIssueById(id);
            if(flaggedIssue == null)
                return NotFound();

            _repo.DeleteFlaggedIssue(flaggedIssue);

            return Accepted();
        }

        private bool FlaggedIssueExists(Guid id)
        {
            var flaggedIssue = _repo.GetFlaggedIssueById(id);
            if (flaggedIssue == null)
                return false;

            return true;
        }
    }
}
