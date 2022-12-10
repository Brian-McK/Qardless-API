using AutoMapper;
using Qardless.API.Dtos.Changelog;
using Qardless.API.Models;

namespace Qardless.API.Profiles
{
    public class ChangelogProfile : Profile
    {
        public ChangelogProfile()
        {
            // GET Full
            CreateMap<Changelog, ChangelogReadDto>();

            // GET Partial
            //CreateMap<>();

            // POST
            CreateMap<ChangelogCreateDto, Changelog>();

            // PUT
            CreateMap<ChangelogUpdateDto, Changelog>();

            // PATCH
            CreateMap<Changelog, ChangelogUpdateDto>();
        }
    }
}
