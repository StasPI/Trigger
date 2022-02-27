using AutoMapper;
using Contracts.Reaction;
using Entities.Reaction;

namespace Mapping.Reaction
{
    public class EmailDestinationMappingsProfile : Profile
    {
        public EmailDestinationMappingsProfile()
        {
            CreateMap<EmailDestination, EmailDestinationDto>();

            CreateMap<EmailDestinationDto, EmailDestination>()
                .ForMember(x => x.Id, map => map.Ignore())
                .ForMember(x => x.DateCreated, map => map.Ignore())
                .ForMember(x => x.DateUpdated, map => map.Ignore())
                .ForMember(x => x.DateDeleted, map => map.Ignore());
        }
    }
}