using AutoMapper;
using Commands;
using EntityFramework;
using EntityFramework.Abstraction;
using MediatR;
using Messages;
using Messages.Abstraction;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Abstraction;
using RabbitMQ.Client;
using System.Reflection;
using WebApi.Worker;
using WebApi.Worker.Options;
using WebApi.Worker.Producer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.
                      GetConnectionString("Postgres");

builder.Services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));

builder.Services.AddDbContext<IDatabaseContext, DatabaseContext>(options =>
    options.UseNpgsql(connectionString), ServiceLifetime.Transient);

builder.Services.AddMediatR(typeof(PostUseCasesCommand).GetTypeInfo().Assembly);

builder.Services.AddControllers();

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

builder.Services.Configure<WorkerOptions>(builder.Configuration.GetSection(WorkerOptions.Name));
builder.Services.Configure<ProducerOptions>(builder.Configuration.GetSection(ProducerOptions.Name));

builder.Services.AddHostedService<WorkerEvents>();
builder.Services.AddSingleton<IEvents, Events>();
builder.Services.AddSingleton<IRabbitMqProducer<EventMessage>, ProducerEvent>();

builder.Services.AddHostedService<WorkerReactions>();
builder.Services.AddSingleton<IReactions, Reactions>();
builder.Services.AddSingleton<IRabbitMqProducer<ReactionMessage>, ProducerReaction>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static MapperConfiguration GetMapperConfiguration()
{
    var configuration = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<Mapping.Registration.UseCasesMappingsProfile>();
    });
    configuration.AssertConfigurationIsValid();
    return configuration;
}