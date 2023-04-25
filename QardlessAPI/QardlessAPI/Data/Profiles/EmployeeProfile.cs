using AutoMapper;
using QardlessAPI.Data.Dtos.Employee;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data.Profiles
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
        }
    }
}
