using Microsoft.EntityFrameworkCore;

namespace EntityFramework
{
    public class DatabaseContext : DbContext
    {
        private readonly string _connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options, string connectionString) : base(options)
        {
            _connectionString = connectionString;
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Sellers> Sellers { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>().HasData(
                new Categories[]
                {
                new Categories { Id=1, Name="Мебель"},
                new Categories { Id=2, Name="Недвижимость"},
                new Categories { Id=3, Name="Техника"},
                new Categories { Id=4, Name="Журналы"},
                new Categories { Id=5, Name="Авто"},
                });

            modelBuilder.Entity<Sellers>().HasData(
                new Sellers[]
                {
                new Sellers { Id=1, FirstName="Иван1", LastName="Иванов1", MiddleName="Иванович1", Email="111@mail.ru", Phone ="1234567"},
                new Sellers { Id=2, FirstName="Иван2", LastName="Иванов2", MiddleName="Иванович2", Email="222@mail.ru", Phone ="2234567"},
                new Sellers { Id=3, FirstName="Иван3", LastName="Иванов3", MiddleName="Иванович3", Email="333@mail.ru", Phone ="3234567"},
                new Sellers { Id=4, FirstName="Иван4", LastName="Иванов4", MiddleName="Иванович4", Email="444@mail.ru", Phone ="4234567"},
                new Sellers { Id=5, FirstName="Иван5", LastName="Иванов5", MiddleName="Иванович5", Email="555@mail.ru", Phone ="5234567"},
                });

            modelBuilder.Entity<Products>().HasData(
                new Products[]
                {
                new Products { Id=1, Name="Стул", Description="Дерево", Price=100, CategoryId=1, SellerId=1 },
                new Products { Id=2, Name="Стол", Description="Дерево", Price=200, CategoryId=1, SellerId=1 },
                new Products { Id=3, Name="Стол", Description="Не дерево", Price=300, CategoryId=1, SellerId=2 },
                new Products { Id=4, Name="Гараж", Description="обычный", Price=30000, CategoryId=2, SellerId=2 },
                new Products { Id=5, Name="Субару", Description="красненький", Price=10000, CategoryId=5, SellerId=5 },
                });
        }
    }
}