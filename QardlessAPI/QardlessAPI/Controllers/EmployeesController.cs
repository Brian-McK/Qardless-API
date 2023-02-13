using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet("/employees")]
        public async Task<ActionResult<IEnumerable<Employee>>> ViewAllEmployees()
        {
          if (_context.Employees == null)
          return NotFound();

            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("/employees/{id}")]
        public async Task<ActionResult<Employee>> ViewEmployeeById(Guid id)
        {
          if (_context.Employees == null)
          return NotFound();

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            return NotFound();

            return employee;
        }

        // PUT: api/Employees/5
        [HttpPut("/employees/{id}")]
        public async Task<IActionResult> EditEmployee(Guid id, Employee employee)
        {
            if (id != employee.Id)
            return BadRequest();

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost("/employees")]
        public async Task<ActionResult<Employee>> AddNewEmployee(Employee employee)
        {
          if (_context.Employees == null)
          return Problem("Entity set 'ApplicationDbContext.Employee'  is null.");

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("/employees/{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            if (_context.Employees == null)
            return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(Guid id)
        {
            return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
