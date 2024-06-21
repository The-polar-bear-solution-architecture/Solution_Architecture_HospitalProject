using EventStore.Client;
using Microsoft.EntityFrameworkCore;
using PatientService.Controllers;
using PatientService.Domain;
using PatientService.DomainServices;
using PatientService.Repository;
using RabbitMQ.Messages.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddTransient<EventStoreRepository>();
builder.Services.AddScoped<IGeneralPractitionerRepository, GeneralPractitionerRepository>();
builder.Services.AddDbContext<PatientDBContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Braphia_PatientService")));

//RabbitMQ
builder.Services.UseRabbitMQMessageHandler(builder.Configuration);
builder.Services.AddHostedService<PatientWorker>();


//EventStore
string eventSourceConnection = builder.Configuration.GetConnectionString("EventSourceDB");
var settings = EventStoreClientSettings.Create(eventSourceConnection);
var client = new EventStoreClient(settings);

builder.Services.AddSingleton(client);

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
