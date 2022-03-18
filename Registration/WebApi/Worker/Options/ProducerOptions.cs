namespace WebApi.Worker.Options
{
    public class ProducerOptions
    {
        public const string Name = "ProducerOptions";
        public string AppId { get; set; }
        public ProducerEventOptions Events { get; set; }
        public ProducerReactionOptions Reactions { get; set; }
    }
}
