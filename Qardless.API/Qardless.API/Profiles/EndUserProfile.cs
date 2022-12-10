using AutoMapper;
using Qardless.API.Dtos.EndUser;
using Qardless.API.Models;

namespace Qardless.API.Profiles
{
    public class EndUserProfile : Profile
    {
        public EndUserProfile()
        {
            // GET Full
            CreateMap<EndUser, EndUserReadFullDto>();

            // GET Partial
            CreateMap<EndUser, EndUserReadPartialDto>();

            // POST
            CreateMap<EndUserCreateDto, EndUser>();

            // PUT
            CreateMap<EndUserUpdateDto, EndUser>();

            // PATCH
            CreateMap<EndUser, EndUserUpdateDto>();
        }
    }
}
