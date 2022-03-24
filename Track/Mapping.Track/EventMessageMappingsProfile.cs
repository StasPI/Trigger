using AutoMapper;
using Dto.Track;
using Entities.Manager;

namespace Mapping.Track
{
    public class EventMessageMappingsProfile : Profile
    {
        public EventMessageMappingsProfile()
        {
             CreateMap<EventMessageReceiveDto, EventMessages>()
                .ForMember(x => x.DateCreated, map => map.Ignore())
                .ForMember(x => x.DateUpdated, map => map.Ignore())
                .ForMember(x => x.DateDeleted, map => map.Ignore());
        }
    }
}