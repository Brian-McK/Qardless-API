using AutoMapper;
using Qardless.API.Dtos.Business;
using Qardless.API.Models;

namespace Qardless.API.Profiles
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            // GET Full
            CreateMap<Business, BusinessReadFullDto>();

            // GET Partial
            CreateMap<Business, BusinessReadPartialDto>();

            // POST
            CreateMap<BusinessCreateDto, Business>();

            // PUT
            CreateMap<BusinessUpdateDto, Business>();

            // PATCH
            CreateMap<Business, BusinessUpdateDto>();
        }


    }
}
