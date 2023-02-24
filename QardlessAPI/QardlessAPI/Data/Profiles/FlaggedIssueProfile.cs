using AutoMapper;
using QardlessAPI.Data.Dtos.Changelog;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data.Profiles
{
    public class FlaggedIssueProfile : Profile
    {
        public FlaggedIssueProfile()
        {
            // GET Full
            CreateMap<FlaggedIssue, ChangelogReadDto>();

            // GET Partial
            //CreateMap<>();

            // POST
            CreateMap<ChangelogCreateDto, FlaggedIssue>();

            // PUT
            CreateMap<ChangelogUpdateDto, FlaggedIssue>();

            // PATCH
            CreateMap<FlaggedIssue, ChangelogUpdateDto>();
        }
    }
}
