using AutoMapper;
using QardlessAPI.Data.Dtos.Business;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data.Profiles
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
