﻿using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;

namespace Commands.Implementation
{
    public class GetByIdUseCasesCommand : UseCasesGetDto, IRequest<UseCasesGetDto>
    {
        public class GetByIdUseCasesCommandHandler : IRequestHandler<GetByIdUseCasesCommand, UseCasesGetDto>
        {
            private readonly ILogger<GetByIdUseCasesCommandHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;
            private List<JsonObject> _caseEvent;
            private List<JsonObject> _caseReaction;

            public GetByIdUseCasesCommandHandler(ILogger<GetByIdUseCasesCommandHandler> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
            {
                _logger = logger;
                IServiceScope scope = scopeFactory.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                _mapper = mapper;
                _caseEvent = new();
                _caseReaction = new();
            }

            public async Task<UseCasesGetDto> Handle(GetByIdUseCasesCommand query, CancellationToken cancellationToken)
            {
                UseCases useCases = await _context.UseCases.Where(x => (x.Id == query.Id) & (x.DateDeleted == null)).FirstAsync(cancellationToken);

                UseCasesGetDto useCasesGetDto = _mapper.Map<UseCasesGetDto>(useCases);

                List<Task> tasks = new()
                {
                    Task.Run(async () => _caseEvent = await ConvertObject.ListStringToJsonObject(useCasesGetDto.CaseEventStr)),
                    Task.Run(async () => _caseReaction = await ConvertObject.ListStringToJsonObject(useCasesGetDto.CaseReactionStr))
                };
                Task.WhenAll(tasks).Wait(cancellationToken);

                useCasesGetDto.CaseEvent = _caseEvent;
                useCasesGetDto.CaseReaction = _caseReaction;

                _logger.LogInformation("GetByIdUseCasesCommandHandler get UseCase {id} : {time}", useCasesGetDto.Id, DateTimeOffset.Now);

                return useCasesGetDto;
            }
        }
    }
}
