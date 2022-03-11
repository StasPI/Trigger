using AutoMapper;
using Commands;
using EntityFramework;
using EntityFramework.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Reflection;
using WebApi.Worker;
using WebApi.Worker.Options;
using Worker;
using Worker.Abstraction;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.
                      GetConnectionString("Postgres");

// Add services to the container.
builder.Services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));

builder.Services.AddDbContext<IDatabaseContext, DatabaseContext>(options =>
    options.UseNpgsql(connectionString), ServiceLifetime.Transient);

builder.Services.AddMediatR(typeof(PostUseCasesCommand).GetTypeInfo().Assembly);

builder.Services.AddControllers();



builder.Services.AddScoped(serviceProvider =>
{
    //amqp://{username}:{password}@{host}:{port}/{virtual_host}
    var uri = new Uri("amqp://sa:Password1@localhost:15800/Events_HOST");
    return new ConnectionFactory
    {
        Uri = uri
    };
});

builder.Services.Configure<WorkerOptions>(builder.Configuration.GetSection(WorkerOptions.Name));

builder.Services.AddSingleton<IEvents, Events>();
builder.Services.AddSingleton<IReactions, Reactions>();

builder.Services.AddHostedService<WorkerEvents>();
//builder.Services.AddHostedService<WorkerReactions>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Configure the HTTP request pipeline.
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