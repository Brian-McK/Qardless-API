using AutoMapper;
using Qardless.API.Dtos.Certificate;
using Qardless.API.Models;

namespace Qardless.API.Profiles
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

            // PATCH
            CreateMap<Certificate, CertificateUpdateDto>();
        }
    }
}
