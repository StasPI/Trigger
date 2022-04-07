using AutoMapper;
using Commands;
using EntityFramework;
using EntityFramework.Abstraction;
using MediatR;
using Messages;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Abstraction;
using RabbitMQ.Client;
using System.Reflection;
using WebApi.Worker;
using WebApi.Worker.Options;
using WebApi.Worker.Producer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Options
string connectionString = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));

builder.Services.AddDbContext<IDatabaseContext, DatabaseContext>(options =>
    options.UseNpgsql(connectionString), ServiceLifetime.Transient);

builder.Services.Configure<WorkerOptions>(builder.Configuration.GetSection(WorkerOptions.Name));
builder.Services.Configure<ProducerOptions>(builder.Configuration.GetSection(ProducerOptions.Name));

//Handlers
builder.Services.AddMediatR(typeof(DeleteUseCasesCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetByIdUseCasesCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(PostUseCasesCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(PutUseCasesCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(EventsMessage).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(ReactionsMessage).GetTypeInfo().Assembly);

//RabbitMessageProducer
builder.Services.AddSingleton<IRabbitMqProducer<EventMessageBody>, ProducerEvent>();
builder.Services.AddSingleton<IRabbitMqProducer<ReactionMessageBody>, ProducerReaction>();

//Workers
builder.Services.AddHostedService<WorkerEvents>();
builder.Services.AddHostedService<WorkerReactions>();

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
