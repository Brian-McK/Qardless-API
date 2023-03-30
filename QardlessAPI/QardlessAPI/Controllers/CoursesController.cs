using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QardlessAPI.Data.Models;
using QardlessAPI.Data.Dtos.Course;
using QardlessAPI.Data;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public CoursesController(IQardlessAPIRepo repo, IMapper mapper)
        {
            _repo = repo ??
                    throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("/courses")]
        public async Task<ActionResult<Course>> AllCourses()
        {
            var courses = await _repo.ListAllCourses();

            if (courses == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<Course>>(courses));
        }

        [HttpGet("/courses/{id}")]
        public async Task<ActionResult<Course>> CourseById(Guid id)
        {
            var course = await _repo.GetCourseById(id);
            return Ok(course);
        }
        
        [HttpGet("/courses/businesses/{id}")]
        public async Task<ActionResult<Course>> CoursesByBusinessId(Guid id)
        {
            if (id == null) return BadRequest();

            var courses = await _repo.ListCoursesByBusinessId(id);

            return Ok(_mapper.Map<IEnumerable<Course>>(courses));
        }

        [HttpPut("/courses/{id}")]
        public async Task<ActionResult> UpdateCourse(Guid id, CourseReadDto courseUpdate)
        {
            if (courseUpdate == null) return BadRequest();

            var course = await _repo.GetCourseById(id);
            if (course == null) return BadRequest();

            await Task.Run(() => _repo.UpdateCourseDetails(id, courseUpdate));

            return Accepted(course);
        }

        [HttpPost("/courses")]
        public async Task<ActionResult<CourseReadDto?>> AddNewCourse(CourseReadDto newCourse)
        {
            if (newCourse == null) return BadRequest();

            var courseReadDto = await Task.Run(() => _repo.AddNewCourse(newCourse));

            return Created("/courses", courseReadDto);
        }

        [HttpDelete("/course/{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _repo.GetCourseById(id);
            if (course == null) return BadRequest();

            _repo.DeleteCourse(course);

            return Accepted();
        }

        private bool CheckCourseExists(Guid id)
        {
            var course = _repo.GetCourseById(id);
            if (course == null)
                return false;

            return true;
        }
    }
}
