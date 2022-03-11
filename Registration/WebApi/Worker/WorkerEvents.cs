using Microsoft.Extensions.Options;
using RabbitMQ.Abstraction;
using WebApi.Worker.Options;
using Worker;
using Worker.Abstraction;

namespace WebApi.Worker
{
    public class WorkerEvents : BackgroundService
    {
        private readonly ILogger<WorkerEvents> _logger;
        private readonly IEvents _events;
        private readonly WorkerOptions _options;
        private readonly IRabbitMqProducer<EventMessage> _producer;

        public WorkerEvents(IRabbitMqProducer<EventMessage> producer, ILogger<WorkerEvents> logger, IEvents events, IOptions<WorkerOptions> options)
        //public WorkerEvents(IServiceScopeFactory scopeFactory, ILogger<WorkerEvents> logger, IEvents events, IOptions<WorkerOptions> options)
        {
            //IServiceScope scope = scopeFactory.CreateScope();
            //_producer = scope.ServiceProvider.GetRequiredService<IRabbitMqProducer<EventMessage>>();
            _producer = producer;
            _logger = logger;
            _events = events;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("WorkerEvents run at: {time}", DateTimeOffset.Now);
                EventMessage eventMessage = new() { EventMessages = await _events.Get(_options.Events.MaxMessages, cancellationToken) };

                _producer.Publish(eventMessage);

                await Task.Delay(_options.Events.DelayMs, cancellationToken);
            }
            await Task.CompletedTask;
        }
    }
}
