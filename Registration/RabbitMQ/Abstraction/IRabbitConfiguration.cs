using RabbitMQ.Client;

namespace RabbitMQ.Abstraction
{
    public interface IRabbitConfiguration
    {
        ConnectionFactory ConnectionFactory { get; }
        string Exchange { get; }
        string Queue { get; }
        string RoutingKey { get; }
        string Type { get; }
        bool Durable { get; }
    }
}
