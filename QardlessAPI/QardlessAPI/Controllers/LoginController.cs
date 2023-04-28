using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Dtos.Authentication;
using QardlessAPI.Data.Models;
using System.Text;
using System.Security.Cryptography;
using AutoMapper;
using QardlessAPI.Data.Dtos.Employee;
using QardlessAPI.Data.Dtos.Admin;

namespace QardlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // Commented out while testing AccountController
        /*
        private readonly IQardlessAPIRepo _repo;

        public LoginController(IQardlessAPIRepo repo)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
        }

        // POST: api/endusers/login/
        [HttpPost("/endusers/login")]
        public async Task<ActionResult<EndUserReadPartialDto>> LoginEndUser(LoginDto loginUser)
        {
            EndUser? endUser = await _repo.GetEndUserByEmail(loginUser);

            if (loginUser == null || endUser == null)
                return BadRequest();

            if (!_repo.CheckEndUserPassword(endUser, loginUser))
                return Unauthorized();
            
            EndUserReadPartialDto endUserForProps = _repo.SendEndUserForProps(endUser);
            return Ok(endUserForProps);
        }


        // POST: api/employees/login/
        [HttpPost("/employees/login")]
        public async Task<ActionResult<EmployeeReadPartialDto>> LoginEmployee(LoginDto loginEmp)
        {
            Employee? emp = await _repo.GetEmployeeByEmail(loginEmp);

            if (loginEmp == null || emp == null)
                return BadRequest();

            if (!_repo.CheckEmpPassword(emp, loginEmp))
                return Unauthorized();

            EmployeeReadPartialDto empForProps = _repo.SendEmpForProps(emp);
            return Ok(empForProps);
        }

        // POST: api/admins/login/
        [HttpPost("/admins/login")]
        public async Task<ActionResult<AdminReadDto>> LoginAdmin(LoginDto loginAdmin)
        {
            Admin? admin = await _repo.GetAdminByEmail(loginAdmin);

            if (loginAdmin == null || admin == null)
                return BadRequest();

            if (!_repo.CheckAdminPassword(admin, loginAdmin))
                return Unauthorized();

            AdminPartialDto adminForProps = _repo.SendAdminForProps(admin);
            return Ok(adminForProps);
        }
        */
    }
}
