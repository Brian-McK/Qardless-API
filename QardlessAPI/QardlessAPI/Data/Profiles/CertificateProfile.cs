using AutoMapper;
using QardlessAPI.Data.Dtos.Certificate;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data.Profiles
{
    public class CertificateProfile : Profile
    {
        public CertificateProfile()
        {
            // GET Full
            CreateMap<Certificate, CertificateReadFullDto>();

            // GET Partial
            CreateMap<Certificate, CertificateReadPartialDto>();

            // POST
            CreateMap<CertificateCreateDto, Certificate>();

            // PUT
            CreateMap<CertificateUpdateDto, Certificate>();
        }
    }
}
