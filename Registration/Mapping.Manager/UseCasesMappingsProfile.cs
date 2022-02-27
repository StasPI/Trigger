using AutoMapper;
using Contracts.Manager;
using Entities.Manager;

namespace Mapping.Manager
{
    public class UseCasesMappingsProfile : Profile
    {
        public UseCasesMappingsProfile()
        {
            CreateMap<UseCases, UseCasesDto>();

            CreateMap<UseCasesDto, UseCases>()
                .ForMember(x => x.Id, map => map.Ignore())
                .ForMember(x => x.DateCreated, map => map.Ignore())
                .ForMember(x => x.DateUpdated, map => map.Ignore())
                .ForMember(x => x.DateDeleted, map => map.Ignore());
        }
    }
}