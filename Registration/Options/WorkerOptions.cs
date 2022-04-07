namespace Options
{
    public class WorkerOptions
    {
        public const string Name = "WorkerOptions";
        public WorkerEventsOptions Events { get; set; }
        public WorkerReactionsOptions Reactions { get; set; }
    }
}
