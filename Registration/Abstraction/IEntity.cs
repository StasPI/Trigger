namespace Abstraction
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
        DateTime? DateDeleted { get; set; }
    }
}