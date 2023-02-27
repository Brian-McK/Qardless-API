using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        // GET: api/Courses
        [HttpGet("/courses")]
        public async Task<ActionResult<Course>> AllCourses()
        {
            var courses = await _repo.ListAllCourses();

            if (courses == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<Course>>(courses));
        }

        // GET: api/Courses/5
        [HttpGet("/courses/{id}")]
        public async Task<ActionResult<Course>> CourseById(Guid id)
        {
            var course = await _repo.GetCourseById(id);

            return Ok(course);
        }

        // PUT: api/Courses/5
        [HttpPut("/courses/{id}")]
        public async Task<ActionResult> UpdateCourse(Guid id, CourseReadDto courseUpdate)
        {
            if (courseUpdate == null) return BadRequest();

            var course = await _repo.GetCourseById(id);
            if (course == null) return BadRequest();

            await Task.Run(() => _repo.UpdateCourseDetails(id, courseUpdate));

            return Accepted(course);
        }

        // POST: api/Courses
        [HttpPost("/courses")]
        public async Task<ActionResult<CourseReadDto?>> AddNewCourse(CourseReadDto newCourse)
        {
            if (newCourse == null) return BadRequest();

            CourseReadDto courseReadDto = await Task.Run(() => _repo.AddNewCourse(newCourse));

            return Created("/courses", courseReadDto);
        }

        // DELETE: api/Course/5
        [HttpDelete("/course/{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _repo.GetCourseById(id);
            if (course == null) return BadRequest();

            _repo.DeleteCourse(course);
            _repo.SaveChanges();

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
