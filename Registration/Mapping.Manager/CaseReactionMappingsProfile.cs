using AutoMapper;
using Contracts.Manager;
using Entities.Manager;

namespace Mapping.Manager
{
    public class CaseReactionMappingsProfile : Profile
    {
        public CaseReactionMappingsProfile()
        {
            CreateMap<CaseReaction, CaseReactionDto>()
                .ForMember(d => d.Destination, map => map.Ignore())
                .ForMember(x => x.UseCasesID, map => map.Ignore());

            CreateMap<CaseReactionDto, CaseReaction>()
                .ForMember(d => d.Id, map => map.Ignore())
                .ForMember(d => d.DateCreated, map => map.Ignore())
                .ForMember(d => d.DateUpdated, map => map.Ignore())
                .ForMember(d => d.DateDeleted, map => map.Ignore());
        }
    }
}
