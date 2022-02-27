using AutoMapper;
using Contracts.Manager;
using Entities.Manager;

namespace Mapping.Manager
{
    public class CaseEventMappingsProfile : Profile
    {
        public CaseEventMappingsProfile()
        {
            CreateMap<CaseEvent, CaseEventDto>()
                .ForMember(x => x.Source, map => map.Ignore())
                .ForMember(x => x.Rule, map => map.Ignore())
                .ForMember(x => x.UseCasesID, map => map.Ignore());

            CreateMap<CaseEventDto, CaseEvent>()
                .ForMember(x => x.Id, map => map.Ignore())
                .ForMember(x => x.DateCreated, map => map.Ignore())
                .ForMember(x => x.DateUpdated, map => map.Ignore())
                .ForMember(x => x.DateDeleted, map => map.Ignore());
        }
    }
}
