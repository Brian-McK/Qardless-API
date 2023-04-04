using AutoMapper;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data.Profiles
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
        }
    }
}
