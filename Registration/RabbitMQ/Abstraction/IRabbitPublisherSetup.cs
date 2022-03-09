namespace RabbitMQ.Abstraction
{
    public interface IRabbitPublisherSetup
    {
        IRabbitPublisher Setup(RabbitConfiguration rabbitConfiguration);
    }
}