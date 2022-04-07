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

namespace Commands
{
    public class PutUseCasesCommand : UseCasesPostDto, IRequest<int>
    {
        public class PutUseCasesCommandHandler : IRequestHandler<PutUseCasesCommand, int>
        {
            private readonly ILogger<PutUseCasesCommandHandler> _logger;
            private readonly IServiceScope _scope;
            private readonly IMapper _mapper;
            private string _caseEvent;
            private string _caseReaction;

            public PutUseCasesCommandHandler(ILogger<PutUseCasesCommandHandler> logger, IServiceScopeFactory scopeFactory)
            {
                _logger = logger;
                _scope = scopeFactory.CreateScope();
            }
            public async Task<int> Handle(PutUseCasesCommand command, CancellationToken cancellationToken)
            {
                IDatabaseContext context = _scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    _logger.LogInformation("PutUseCasesCommandHandler Put UseCase: {id} | Time: {time}", command.Id, DateTimeOffset.Now);
                    UseCases useCases = await context.UseCases.Where(x => (x.Id == command.Id) & (x.DateDeleted == null)).FirstAsync(cancellationToken);

                    List<Task> tasks = new()
                    {
                        Task.Run(async () => _caseEvent = await ConvertObject.JsonObjectToStringAsync(command.CaseEvent)),
                        Task.Run(async () => _caseReaction = await ConvertObject.JsonObjectToStringAsync(command.CaseReaction))
                    };
                    Task.WhenAll(tasks).Wait(cancellationToken);

                    if (!useCases.CaseEvent.SequenceEqual(_caseEvent))
                    {
                        useCases.SendEvent = false;
                    }

                    if (!useCases.CaseReaction.SequenceEqual(_caseReaction))
                    {
                        useCases.SendReaction = false;
                    }

                    useCases.DateUpdated = DateTime.UtcNow;
                    useCases.UserId = command.UserId;
                    useCases.CaseName = command.CaseName;
                    useCases.CaseEvent = _caseEvent;
                    useCases.CaseReaction = _caseReaction;
                    useCases.Active = command.Active;

                    await context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    return useCases.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogWarning("PutUseCasesCommandHandler Exception UseCase: {id} | Time: {time} | Error: {ex} :", command.Id, DateTimeOffset.Now, ex);
                    return -1;
                }
            }
        }
    }
}
