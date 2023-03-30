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
            var flaggedIssues = await _repo.GetFlaggedIssues();

            if (flaggedIssues == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<FlaggedIssueReadDto>>(flaggedIssues));
        }

        [HttpGet("/flaggedissues/{id}")]
        public async Task<ActionResult<FlaggedIssue>> FlaggedIssueById(Guid id)
        {
            var flaggedIssue = await _repo.GetFlaggedIssue(id);

            if (flaggedIssue == null)
                return NotFound();

            return Ok(_mapper.Map<FlaggedIssueReadDto>(flaggedIssue));
        }

        [HttpPut("/flaggedissues/{id}")]
        public async Task<ActionResult> UpdateFlaggedIssueWasRead(Guid id, FlaggedIssueUpdateDto flaggedIssueUpdateDto)
        {
            if (flaggedIssueUpdateDto == null)
                return BadRequest();

            var flaggedIssue = await _repo.GetFlaggedIssue(id);
            if (flaggedIssue == null)
                return NotFound();

            flaggedIssue.WasRead = true;

            _mapper.Map(flaggedIssueUpdateDto, flaggedIssue);
            _repo.UpdateFlaggedIssue(id, flaggedIssue);

            return Accepted();
        }

        // POST: api/FlaggedIssue
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

        // DELETE: api/FlaggedIssue/5
        [HttpDelete("/flaggedissues/{id}")]
        public async Task<IActionResult> DeleteFlaggedIssue(Guid id)
        {
            var flaggedIssue = await _repo.GetFlaggedIssue(id);
            if(flaggedIssue == null)
                return NotFound();

            _repo.DeleteFlaggedIssue(flaggedIssue);

            return Accepted();
        }

        private bool FlaggedIssueExists(Guid id)
        {
            var flaggedIssue = _repo.GetFlaggedIssue(id);
            if (flaggedIssue == null)
                return false;

            return true;
        }
    }
}
