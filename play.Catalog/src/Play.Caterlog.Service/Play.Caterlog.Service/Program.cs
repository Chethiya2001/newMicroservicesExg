
using Play.Caterlog.Service.Entities;
using Play.Common.Settings;
using Play.MongoDB.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mongo Config
ServiceSettings serviceSettings = new ServiceSettings();
var configuration = builder.Configuration;
serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!;

builder.Services.AddMongo().AddMongoRepository<Item>("items");

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
