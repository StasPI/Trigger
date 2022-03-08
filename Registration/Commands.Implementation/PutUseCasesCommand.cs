﻿using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Commands.Implementation
{
    public class PutUseCasesCommand : UseCasesPostDto, IRequest<int>
    {
        public class PutUseCasesCommandHandler : IRequestHandler<PutUseCasesCommand, int>
        {
            private readonly ILogger<PutUseCasesCommandHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;
            private List<string> _caseEvent;
            private List<string> _caseReaction;

            public PutUseCasesCommandHandler(ILogger<PutUseCasesCommandHandler> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
            {
                _logger = logger;
                IServiceScope scope = scopeFactory.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                _mapper = mapper;
                _caseEvent = new();
                _caseReaction = new();
            }
            public async Task<int> Handle(PutUseCasesCommand command, CancellationToken cancellationToken)
            {
                using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    UseCases useCases = await _context.UseCases.Where(x => (x.Id == command.Id) & (x.DateDeleted == null)).FirstAsync(cancellationToken);

                    List<Task> tasks = new()
                    {
                        Task.Run(async () => _caseEvent = await ConvertObject.ListJsonObjectToListString(command.CaseEvent)),
                        Task.Run(async () => _caseReaction = await ConvertObject.ListJsonObjectToListString(command.CaseReaction))
                    };
                    Task.WhenAll(tasks).Wait(cancellationToken);

                    if (useCases.CaseName != command.CaseName)
                    {
                        useCases.CaseName = command.CaseName;
                    }
                    
                    if (!useCases.CaseEvent.SequenceEqual(_caseEvent))
                    {
                        useCases.CaseEvent = _caseEvent;
                        useCases.SendEvent = false;
                    }

                    if (!useCases.CaseReaction.SequenceEqual(_caseReaction))
                    {
                        useCases.CaseReaction = _caseReaction;
                        useCases.SendReaction = false;
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    _logger.LogInformation("PutUseCasesCommandHandler update UseCase {id} : {time}", useCases.Id, DateTimeOffset.Now);
                    return useCases.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogInformation("PutUseCasesCommandHandler Error: {ex} : {time}", ex, DateTimeOffset.Now);
                    return -1;
                }
            }
        }
    }
}