using AutoMapper;
using Qardless.API.Dtos.Employee;
using Qardless.API.Models;

namespace Qardless.API.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // GET Full
            CreateMap<Employee, EmployeeReadFullDto>();

            // GET Partial
            CreateMap<Employee, EmployeeReadPartialDto>();

            // POST
            CreateMap<EmployeeCreateDto, Employee>();

            // PUT
            CreateMap<EmployeeUpdateDto, Employee>();

            // PATCH
            CreateMap<Employee, EmployeeUpdateDto>();
        }
    }
}
