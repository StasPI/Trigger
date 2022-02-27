using AutoMapper;
using Contracts.Event;
using Entities.Event;

namespace Mapping.Event
{
    public class EmailRuleMappingsProfile : Profile
    {
        public EmailRuleMappingsProfile()
        {
            CreateMap<EmailRule, EmailRuleDto>();

            CreateMap<EmailRuleDto, EmailRule>()
                .ForMember(x => x.Id, map => map.Ignore())
                .ForMember(x => x.DateCreated, map => map.Ignore())
                .ForMember(x => x.DateUpdated, map => map.Ignore())
                .ForMember(x => x.DateDeleted, map => map.Ignore());
        }
    }
}
