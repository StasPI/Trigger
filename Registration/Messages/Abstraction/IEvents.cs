﻿using Dto.Registration;

namespace Messages.Abstraction
{
    public interface IEvents
    {
        public Task<List<UseCasesSendEventDto>> GetMessageAsync(int maxMessagesEvents, CancellationToken cancellationToken);
        public Task CommitAsync(CancellationToken cancellationToken);
        public Task RollbackAsync(CancellationToken cancellationToken);
    }
}