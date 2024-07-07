
using MassTransit;
using Play.Caterlog.Service.Entities;
using Play.Common.MassTransit;
using Play.Common.Settings;
using Play.MongoDB.Common;

var builder = WebApplication.CreateBuilder(args);

//settings configs
var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MassTransit-RabbitMQ Config
// Mongo Config

builder.Services.AddMongo().AddMongoRepository<Item>("items").AddMassTransitWithRabbitMq();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
