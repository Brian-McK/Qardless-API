using AutoMapper;
using Qardless.API.Dtos.Admin;
using Qardless.API.Models;

namespace Qardless.API.Profiles
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
