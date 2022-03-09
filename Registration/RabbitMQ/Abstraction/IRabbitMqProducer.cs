namespace RabbitMQ.Abstraction
{
    public interface IRabbitMqProducer<in T>
    {
        void Publish(T @event);
    }
}
