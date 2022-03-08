using AutoMapper;
using Dto.Registration;
using Entities.Registration;

namespace Mapping.Registration
{
    public class UseCasesMappingsProfile : Profile
    {
        public UseCasesMappingsProfile()
        {
            CreateMap<UseCases, UseCasesGetDto>()
                .ForMember(x => x.CaseEvent, map => map.Ignore())
                .ForMember(x => x.CaseReaction, map => map.Ignore())
                .ForMember(x => x.CaseEventStr, map => map.MapFrom(src => src.CaseEvent))
                .ForMember(x => x.CaseReactionStr, map => map.MapFrom(src => src.CaseReaction));

            CreateMap<UseCasesPostDto, UseCases>()
                .ForMember(x => x.Id, map => map.Ignore())
                .ForMember(x => x.DateCreated, map => map.Ignore())
                .ForMember(x => x.DateUpdated, map => map.Ignore())
                .ForMember(x => x.DateDeleted, map => map.Ignore())
                .ForMember(x => x.SendEvent, map => map.Ignore())
                .ForMember(x => x.SendReaction, map => map.Ignore())
                .ForMember(x => x.CaseEvent, map => map.MapFrom(src => src.CaseEventStr))
                .ForMember(x => x.CaseReaction, map => map.MapFrom(src => src.CaseReactionStr));

            CreateMap<UseCases, UseCasesSendEventDto>()
                .ForMember(x => x.CaseEvent, map => map.Ignore())
                .ForMember(x => x.CaseEventStr, map => map.MapFrom(src => src.CaseEvent));

            CreateMap<UseCases, UseCasesSendReactionDto>()
                .ForMember(x => x.CaseReaction, map => map.Ignore())
                .ForMember(x => x.CaseReactionStr, map => map.MapFrom(src => src.CaseReaction));
        }
    }
}