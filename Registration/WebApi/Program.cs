using Extensions;
using Modules;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterModule<DataAccessModule>(builder.Configuration);
builder.Services.RegisterModule<MediatrModule>(builder.Configuration);
builder.Services.RegisterModule<MapperModule>(builder.Configuration);
builder.Services.RegisterModule<WorkerModule>(builder.Configuration);
builder.Services.RegisterModule<RabbitModule>(builder.Configuration);

builder.Services.AddControllers();

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