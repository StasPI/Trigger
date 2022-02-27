using AutoMapper;
using Contracts.Event;
using Entities.Event;

namespace Mapping.Event
{
    public class EmailSourceMappingsProfile : Profile
    {
        public EmailSourceMappingsProfile()
        {
            CreateMap<EmailSource, EmailSourceDto>();

            CreateMap<EmailSourceDto, EmailSource>()
                .ForMember(x => x.Id, map => map.Ignore())
                .ForMember(x => x.DateCreated, map => map.Ignore())
                .ForMember(x => x.DateUpdated, map => map.Ignore())
                .ForMember(x => x.DateDeleted, map => map.Ignore());
        }
    }
}