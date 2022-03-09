using AutoMapper;
using Commands;
using EntityFramework;
using EntityFramework.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApi.Worker;
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

builder.Services.Configure<WorkerManagerOptions>(builder.Configuration.GetSection(WorkerManagerOptions.Name));
builder.Services.Configure<EventWorkerOptions>(builder.Configuration.GetSection(EventWorkerOptions.Name));
builder.Services.Configure<ReactionWorkerOptions>(builder.Configuration.GetSection(ReactionWorkerOptions.Name));

builder.Services.AddSingleton<IEventWorker, EventWorker>();
builder.Services.AddSingleton<IReactionWorker, ReactionWorker>();
builder.Services.AddHostedService<WorkerManager>();

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