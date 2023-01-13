using AutoMapper;
using QardlessAPI.Data.Dtos.Admin;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data.Profiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            // Source -> Target

            // GET
            CreateMap<Admin, AdminReadDto>();

            // POST
            CreateMap<AdminCreateDto, Admin>();

            // PUT // NOTE: May remove PUT in favour of PATCH
            CreateMap<AdminUpdateDto, Admin>();

            // PATCH
            CreateMap<Admin, AdminUpdateDto>();
        }
    }
}
