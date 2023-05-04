using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.Employee;
using QardlessAPI.Data.Dtos.Authentication;
using QardlessAPI.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;
        // TODO, add DI below to repo
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtHandler _jwtHandler;

        public EmployeesController(
            IQardlessAPIRepo repo,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            JwtHandler jwtHandler
            )
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));

            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        // GET: api/Employees
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Employee>> AllEmployees()
        {
            var employees = await _repo.ListAllEmployees();

            if (employees == null) 
                return NotFound();

            return Ok(employees);
        }

        // GET: api/Employees/5

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Employee>> ViewEmployeeById(Guid id)
        {
            var emp = await _repo.GetEmployeeById(id);

            if( emp ==null) 
                return BadRequest();

            return Ok(emp);
        }

        //Task<IEnumerable<Employee?>> GetEmployeesById(Guid id); //TODO: Businesses controller

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateEmployeeDetails(Guid id, EmployeeUpdateDto employeeUpdateDto)
        {
            if(employeeUpdateDto == null )
                return BadRequest();

            var emp = await _repo.GetEmployeeById(id);
            if(emp == null)
                return BadRequest();

            await Task.Run(() => _repo.UpdateEmployee(id, employeeUpdateDto));

            return Accepted(emp);
        }

        // Register Employee
        // POST: api/Employees
        [HttpPost]
        [Authorize(Roles = "Administrator,Business")]
        public async Task<ActionResult<EmployeeCreateDto?>> RegisterNewEmployee(EmployeeCreateDto employeeCreateDto)
        {
            if (employeeCreateDto == null)
                return BadRequest();

            EmployeeReadFullDto empReadFullDto = await Task.Run(() => _repo.AddNewEmployee(employeeCreateDto));

            return Created("/employees", empReadFullDto);
        }

        // Business logic: Logout Employee
        // POST: api/Employees
        [HttpPost("logout")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<LogoutResponseDto>> LogoutEmployee(
            [FromBody] LogoutRequestDto employeeLogoutRequest)
        {
            var emp = await _repo.GetEmployeeById(employeeLogoutRequest.Id);
            if (emp == null)
                return BadRequest();

            LogoutResponseDto employeeLogoutResponse = new LogoutResponseDto
            {
                Id = employeeLogoutRequest.Id,
                IsLoggedIn = false
            };

            return Ok(employeeLogoutResponse);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var emp = await _repo.GetEmployeeById(id);
            if (emp == null)
                return NotFound();

            _repo.DeleteEmployee(emp);
            _repo.SaveChanges();

            return Accepted();
        }

        private bool CheckEmployeeExists(Guid id)
        {
            var emp = _repo.GetEmployeeById(id);
            if (emp == null)
                return false;
            return true;
        }
    }
}
