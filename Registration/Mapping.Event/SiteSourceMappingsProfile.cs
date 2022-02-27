using AutoMapper;
using Contracts.Event;
using Entities.Event;

namespace Mapping.Event
{
    public class SiteSourceMappingsProfile : Profile
    {
        public SiteSourceMappingsProfile()
        {
            CreateMap<SiteSource, SiteSourceDto>();

            CreateMap<SiteSourceDto, SiteSource>()
                .ForMember(x => x.Id, map => map.Ignore())
                .ForMember(x => x.DateCreated, map => map.Ignore())
                .ForMember(x => x.DateUpdated, map => map.Ignore())
                .ForMember(x => x.DateDeleted, map => map.Ignore());
        }
    }
}
