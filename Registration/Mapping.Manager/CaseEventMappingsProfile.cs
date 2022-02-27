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
                .ForMember(d => d.Source, map => map.Ignore())
                .ForMember(d => d.Rule, map => map.Ignore());

            CreateMap<CaseEventDto, CaseEvent>()
                .ForMember(x => x.Id, map => map.Ignore())
                .ForMember(x => x.DateCreated, map => map.Ignore())
                .ForMember(x => x.DateUpdated, map => map.Ignore())
                .ForMember(x => x.DateDeleted, map => map.Ignore());

            //CreateMap<List<CaseEvent>, List<CaseEventDto>>()
            //    .ForMember(d => d.Source, map => map.Ignore())
            //    .ForMember(d => d.Rule, map => map.Ignore());

            //CreateMap<CaseEventDto, CaseEvent>()
            //    .ForMember(x => x.Id, map => map.Ignore())
            //    .ForMember(x => x.DateCreated, map => map.Ignore())
            //    .ForMember(x => x.DateUpdated, map => map.Ignore())
            //    .ForMember(x => x.DateDeleted, map => map.Ignore());
        }
    }
}
