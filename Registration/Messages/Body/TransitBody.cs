using Microsoft.EntityFrameworkCore.Storage;

namespace Messages
{
    public class TransitBody<T> where T : class
    {
        public IDbContextTransaction Transaction { get; set; }
        public List<T> Messages { get; set; }
    }
}
