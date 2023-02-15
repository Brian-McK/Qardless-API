using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.Changelog;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangelogsController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public ChangelogsController(IQardlessAPIRepo repo, IMapper mapper)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/Changelogs
        [HttpGet("/changelogs")]
        public async Task<ActionResult<Changelog>> ViewAllChangelogs()
        {
            var changeLogs = await _repo.GetChangelogs();

            if (changeLogs == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<ChangelogReadDto>>(changeLogs));
        }

        // GET: api/Changelogs/5
        [HttpGet("/changelogs/{id}")]
        public async Task<ActionResult<Changelog>> ViewChangelogById(Guid id)
        {
            var changelog = await _repo.GetChangelog(id);

            if (changelog == null)
                return NotFound();

            return Ok(_mapper.Map<ChangelogReadDto>(changelog));
        }

        // PUT: api/Changelogs/5
        [HttpPut("/changelogs/{id}")]
        public async Task<IActionResult> UpdateChangelogWasRead(Guid id, ChangelogUpdateDto changelogUpdateDto)
        {
            if (changelogUpdateDto == null)
                return BadRequest();

            var changelog = await _repo.GetChangelog(id);
            if (changelog == null)
                return NotFound();

            changelog.WasRead = true;

            _mapper.Map(changelogUpdateDto, changelog);
            _repo.PutChangelog(id, changelog);
            _repo.SaveChanges();

            return Accepted();
        }

        // POST: api/Changelogs
        [HttpPost("/changelogs")]
        public async Task<ActionResult<Changelog>> AddNewChangelog(ChangelogCreateDto changelogForCreation)
        {
            if(changelogForCreation == null)
                return BadRequest();

            var changelog = _mapper.Map<Changelog>(changelogForCreation);

            changelog.Id = Guid.NewGuid();
            changelog.Type = changelogForCreation.Type;
            changelog.Content = changelogForCreation.Content;
            changelog.WasRead = false;
            changelog.CreatedDate = DateTime.Now;

            _repo.PostChangelog(changelog);
            _repo.SaveChanges();

            return CreatedAtAction("GetChangelogById", new { id = changelog.Id }, changelog);
        }

        // DELETE: api/Changelogs/5
        [HttpDelete("/changelogs/{id}")]
        public async Task<IActionResult> DeleteChangelog(Guid id)
        {
            var changelog = await _repo.GetChangelog(id);
            if(changelog == null)
                return NotFound();

            _repo.DeleteChangelog(changelog);
            _repo.SaveChanges();

            return Accepted();
        }

        private bool ChangelogExists(Guid id)
        {
            var changelog = _repo.GetChangelog(id);
            if (changelog == null)
                return false;

            return true;
        }
    }
}
