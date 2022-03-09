namespace Worker.Abstraction
{
    public interface IReactionWorker
    {
        public Task Run(CancellationToken cancellationToken);
    }
}
