using RabbitMQ;
using RabbitMQ.Client;

namespace RabbitMQ.Implementation
{
    public class RabbitConfiguration : IRabbitConfiguration
    {
        public RabbitConfiguration(ConnectionFactory connectionFactory, string exchange, string routingKey, string type, bool durable, string queue = "")
        {
            ConnectionFactory = connectionFactory;
            Exchange = exchange;
            Queue = queue;
            RoutingKey = routingKey;
            Type = type;
            Durable = durable;
        }
    }
}