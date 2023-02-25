using AutoMapper;
using QardlessAPI.Data.Dtos.FlaggedIssue;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data.Profiles
{
    public class FlaggedIssueProfile : Profile
    {
        public FlaggedIssueProfile()
        {
            // GET Full
            CreateMap<FlaggedIssue, FlaggedIssueReadDto>();

            // GET Partial
            //CreateMap<>();

            // POST
            CreateMap<FlaggedIssueCreateDto, FlaggedIssue>();

            // PUT
            CreateMap<FlaggedIssueUpdateDto, FlaggedIssue>();

            // PATCH
            CreateMap<FlaggedIssue, FlaggedIssueUpdateDto>();
        }
    }
}
