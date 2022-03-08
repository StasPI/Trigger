namespace Worker.Abstraction
{
    public interface IEventWorker
    {
       public Task Run(CancellationToken cancellationToken);
    }
}