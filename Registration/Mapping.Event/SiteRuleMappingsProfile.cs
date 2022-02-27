using AutoMapper;
using Contracts.Event;
using Entities.Event;

namespace Mapping.Event
{
    public class SiteRuleMappingsProfile : Profile
    {
        public SiteRuleMappingsProfile()
        {
            CreateMap<SiteRule, SiteRuleDto>();

            CreateMap<SiteRuleDto, SiteRule>()
                .ForMember(x => x.Id, map => map.Ignore())
                .ForMember(x => x.DateCreated, map => map.Ignore())
                .ForMember(x => x.DateUpdated, map => map.Ignore())
                .ForMember(x => x.DateDeleted, map => map.Ignore());
        }
    }
}
