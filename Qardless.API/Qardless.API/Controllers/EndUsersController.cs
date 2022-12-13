using AutoMapper;
using Qardless.API.Models;
using Qardless.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qardless.API.Dtos.EndUser;

namespace Qardless.API.Controllers
{
    [Route("api/enduser")]
    [ApiController]
    public class EndUsersController : Controller
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public EndUsersController(IQardlessAPIRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        //  NOTE: Starting out with a sync API for simplicity
        [HttpGet]
        public ActionResult<IEnumerable<EndUserReadFullDto>> GetAllEndUsers()
        {
            var endUserItems = _repo.GetEndUsers();
            return Ok(_mapper.Map<IEnumerable<EndUserReadFullDto>>(endUserItems));
        }

        [HttpGet("{id}")]
        public ActionResult<EndUserReadPartialDto> GetEndUserById(Guid id)
        {
            var endUserItem = _repo.GetEndUser(id);
            if (endUserItem != null)
                return Ok(_mapper.Map<EndUserReadPartialDto>(endUserItem));

            return NotFound();
        }

        [HttpPost]
        public ActionResult<EndUserReadFullDto> CreateEndUser(EndUserCreateDto endUserCreateDto)
        {
            var endUserModel = _mapper.Map<EndUser>(endUserCreateDto);
            _repo.CreateEndUser(endUserModel);
            _repo.SaveChanges();

            var endUserReadFullDto = _mapper.Map<EndUserReadFullDto>(endUserModel);

            //return CreatedAtRoute(nameof(GetEndUserById), new { Id = endUserReadFullDto.Id }, endUserReadFullDto);
            return Ok(endUserReadFullDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateEndUser(Guid id, EndUserUpdateDto endUserUpdateDto)
        {
            var endUserModelFromRepo = _repo.GetEndUser(id);
            if (endUserModelFromRepo == null)
                return NotFound();

            _mapper.Map(endUserUpdateDto, endUserModelFromRepo);
            _repo.UpdateEndUser(endUserModelFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }

        
        [HttpPatch("{id}")]
        public ActionResult PartialEndUserUpdate(Guid id, JsonPatchDocument<EndUserUpdateDto> patchDoc)
        {
            var userEndModelFromRepo = _repo.GetEndUser(id);

            if (userEndModelFromRepo == null)
            {
                return NotFound();
            }

            var userToPatch = _mapper.Map<EndUserUpdateDto>(userEndModelFromRepo);
            patchDoc.ApplyTo(userToPatch, ModelState);
            if (!TryValidateModel(userToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(userToPatch, userEndModelFromRepo);
            _repo.UpdateEndUser(userEndModelFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEndUser(Guid id)
        {
            var userModelFromRepo = _repo.GetEndUser(id);

            if (userModelFromRepo == null)
            {
                return NotFound();
            }

            _repo.DeleteEndUser(userModelFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }


        #region Async Code
        // GET: EndUsers
        /*
        public async Task<IActionResult> Index()
        {
              return View(await _context.EndUsers.ToListAsync());
        }
        
        // GET: EndUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.EndUsers == null)
            {
                return NotFound();
            }

            var endUser = await _context.EndUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endUser == null)
            {
                return NotFound();
            }

            return View(endUser);
        }

        // GET: EndUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EndUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Email,EmailVerified,PasswordHash,PhoneMobile,PhoneMobileVerified,PhoneHome,AddressCode,AddressDetailed,CreatedDate,LastLoginDate")] EndUser endUser)
        {
            if (ModelState.IsValid)
            {
                endUser.Id = Guid.NewGuid();
                _context.Add(endUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(endUser);
        }

        // GET: EndUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.EndUsers == null)
            {
                return NotFound();
            }

            var endUser = await _context.EndUsers.FindAsync(id);
            if (endUser == null)
            {
                return NotFound();
            }
            return View(endUser);
        }

        // POST: EndUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,MiddleName,LastName,Email,EmailVerified,PasswordHash,PhoneMobile,PhoneMobileVerified,PhoneHome,AddressCode,AddressDetailed,CreatedDate,LastLoginDate")] EndUser endUser)
        {
            if (id != endUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(endUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EndUserExists(endUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(endUser);
        }

        // GET: EndUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.EndUsers == null)
            {
                return NotFound();
            }

            var endUser = await _context.EndUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endUser == null)
            {
                return NotFound();
            }

            return View(endUser);
        }

        // POST: EndUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.EndUsers == null)
            {
                return Problem("Entity set 'QardlessAPIContext.EndUsers'  is null.");
            }
            var endUser = await _context.EndUsers.FindAsync(id);
            if (endUser != null)
            {
                _context.EndUsers.Remove(endUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EndUserExists(Guid id)
        {
          return _context.EndUsers.Any(e => e.Id == id);
        }
        */
        #endregion
    }
}
