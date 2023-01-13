using AutoMapper;
using QardlessAPI.Data.Dtos.Changelog;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data.Profiles
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
