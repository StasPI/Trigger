namespace RabbitMQ.Abstraction
{
    public interface IRabbitPublisher : IRabbitPublisherSetup
    {
        void Publish(IEnumerable<Payload> payloads);
    }
}
