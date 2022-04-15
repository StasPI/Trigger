namespace Options
{
    public class RabbitMQOptions
    {
        public const string Name = "RabbitMQ";
        public string AppId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public ProducerEventOptions ProducerEvent { get; set; }
        public ProducerReactionOptions ProducerReaction { get; set; }
    }
}
