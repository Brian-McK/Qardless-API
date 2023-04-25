using AutoMapper;
using QardlessAPI.Data.Dtos.Admin;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data.Profiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            // GET
            CreateMap<Admin, AdminReadDto>();

            // POST
            CreateMap<AdminCreateDto, Admin>();

            // PUT 
            CreateMap<AdminUpdateDto, Admin>();
        }
    }
}
