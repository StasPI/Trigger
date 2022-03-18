using EntityFramework;
using EntityFramework.Abstraction;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.
                      GetConnectionString("Postgres");

builder.Services.AddDbContext<IDatabaseContext, DatabaseContext>(options =>
    options.UseNpgsql(connectionString), ServiceLifetime.Transient);

builder.Services.AddSingleton(serviceProvider =>
{
    return new ConnectionFactory
    {
        UserName = "sa",
        Password = "Password1",
        HostName = "localhost",
        Port = 5800,
        VirtualHost = "/",
        ContinuationTimeout = new TimeSpan(10, 0, 0, 0),
    };
});

var app = builder.Build();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
