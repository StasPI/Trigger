using AutoMapper;
using Case.Abstraction;
using Commands.Implementation;
using CreateCase.Implementation;
using EntityFramework.Abstraction;
using EntityFramework.Implementation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApi.Worker;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.
                      GetConnectionString("DBConnection");

// Add services to the container.
builder.Services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));

builder.Services.AddDbContext<IDatabaseContext, DatabaseContext>(options =>
    options.UseNpgsql(connectionString), ServiceLifetime.Transient);

builder.Services.AddMediatR(typeof(CreateUseCasesCommand).GetTypeInfo().Assembly);
builder.Services.AddSingleton<ICreateEvent, CreateEvent>();
builder.Services.AddControllers();

//builder.Services.AddHostedService<WorkerManager>();

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
        cfg.AddProfile<Mapping.Manager.UseCasesMappingsProfile>();
        cfg.AddProfile<Mapping.Manager.CaseReactionMappingsProfile>();
        cfg.AddProfile<Mapping.Manager.CaseEventMappingsProfile>();
        cfg.AddProfile<Mapping.Event.EmailSourceMappingsProfile>();
        cfg.AddProfile<Mapping.Event.SiteSourceMappingsProfile>();
        cfg.AddProfile<Mapping.Event.EmailRuleMappingsProfile>();
        cfg.AddProfile<Mapping.Event.SiteRuleMappingsProfile>();
        cfg.AddProfile<Mapping.Reaction.EmailDestinationMappingsProfile>();
    });
    configuration.AssertConfigurationIsValid();
    return configuration;
}