using AutoMapper;
using Contracts.Implementation;
using Entities.Manager;

namespace Mapping
{
    public class UseCasesMappingsProfile : Profile
    {
        public UseCasesMappingsProfile()
        {
            CreateMap<UseCases, UseCasesDto>();

            CreateMap<UseCasesDto, UseCases>()
                .ForMember(d => d.Id, map => map.Ignore())
                .ForMember(d => d.DateCreated, map => map.Ignore())
                .ForMember(d => d.DateUpdated, map => map.Ignore())
                .ForMember(d => d.DateDeleted, map => map.Ignore());
        }
    }
}