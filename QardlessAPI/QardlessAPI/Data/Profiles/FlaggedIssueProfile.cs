using AutoMapper;
using QardlessAPI.Data.Dtos.FlaggedIssue;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data.Profiles
{
    public class FlaggedIssueProfile : Profile
    {
        public FlaggedIssueProfile()
        {
            // GET 
            CreateMap<FlaggedIssue, FlaggedIssueReadDto>();

            // POST
            CreateMap<FlaggedIssueCreateDto, FlaggedIssue>();

            // PUT
            CreateMap<FlaggedIssueUpdateDto, FlaggedIssue>();
        }
    }
}
