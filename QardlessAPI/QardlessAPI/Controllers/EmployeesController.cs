using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.Employee;
using QardlessAPI.Data.Dtos.Authentication;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IQardlessAPIRepo _repo;
        private readonly IMapper _mapper;

        public EmployeesController(IQardlessAPIRepo repo, IMapper mapper)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/Employees
        [HttpGet("/employees")]
        public async Task<ActionResult<Employee>> AllEmployees()
        {
            var employees = await _repo.ListAllEmployees();

            if (employees == null) 
                return NotFound();

            return Ok(employees);
        }

        // GET: api/Employees/5
        [HttpGet("/employees/{id}")]
        public async Task<ActionResult<Employee>> ViewEmployeeById(Guid id)
        {
            var emp = await _repo.GetEmployeeById(id);

            if( emp ==null) 
                return BadRequest();

            return Ok(emp);
        }

        //Task<IEnumerable<Employee?>> GetEmployeesById(Guid id); //TODO: Businesses controller

        // PUT: api/Employees/5
        [HttpPut("/employees/{id}")]
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
        [HttpPost("/employees")]
        public async Task<ActionResult<EmployeeCreateDto?>> RegisterNewEmployee(EmployeeCreateDto employeeCreateDto)
        {
            if (employeeCreateDto == null)
                return BadRequest();

            EmployeeReadPartialDto empReadPartialDto = await Task.Run(() => _repo.AddNewEmployee(employeeCreateDto));

            return Created("/employees", empReadPartialDto);
        }

        // Business logic: Logout Employee
        // POST: api/Employees
        [HttpPost("/employees/logout")]
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
        [HttpDelete("/employees/{id}")]
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
