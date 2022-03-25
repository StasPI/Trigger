using AutoMapper;
using Commands;
using EntityFramework;
using EntityFramework.Abstraction;
using MediatR;
using Messages;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Reflection;
using WebApi.Consumer;
using WebApi.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.
                      GetConnectionString("Postgres");

builder.Services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));

builder.Services.AddDbContext<IDatabaseContext, DatabaseContext>(options =>
    options.UseNpgsql(connectionString), ServiceLifetime.Transient);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

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
        DispatchConsumersAsync = true
    };
});

builder.Services.Configure<ConsumerOptions>(builder.Configuration.GetSection(ConsumerOptions.Name));
builder.Services.AddTransient<IRequestHandler<EventMessage, Unit>, EventMessageHandler>();
builder.Services.AddHostedService<ConsumerRegistration>();

builder.Services.Configure<ProducerOptions>(builder.Configuration.GetSection(ProducerOptions.Name));


var app = builder.Build();

app.Run();

static MapperConfiguration GetMapperConfiguration()
{
    var configuration = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<Mapping.Track.EventMessageMappingsProfile>();
    });
    configuration.AssertConfigurationIsValid();
    return configuration;
}