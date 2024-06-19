using CheckInService.CommandHandlers;
using CheckInService.Controllers;
using CheckInService.DBContexts;
using CheckInService.Repositories;
using EventStore.Client;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Messages.Configuration;
using RabbitMQ.Messages.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string CheckInDB = builder.Configuration.GetConnectionString("CheckInDB");
string eventSourceConnection = builder.Configuration.GetConnectionString("EventSourceDB");

//-|| Regular database || Configuration
builder.Services.AddDbContext<CheckInContextDB>(options => options.UseSqlServer(CheckInDB), ServiceLifetime.Singleton);
var settings = EventStoreClientSettings.Create(eventSourceConnection);
var client = new EventStoreClient(settings);

// Service via dependency injection
builder.Services.AddTransient<CheckInRepository, CheckInRepository>();
builder.Services.AddTransient<AppointmentRepository, AppointmentRepository>();
builder.Services.AddTransient<PhysicianRepo, PhysicianRepo>();
builder.Services.AddTransient<PatientRepo, PatientRepo>();
builder.Services.AddTransient<CheckInCommandHandler, CheckInCommandHandler>();
builder.Services.AddTransient<EventStoreRepository, EventStoreRepository>();
builder.Services.AddTransient<ReplayHandler, ReplayHandler>();

// Use rabbitMQ Publisher
builder.Services.UseRabbitMQMessagePublisher(builder.Configuration);
builder.Services.UseRabbitMQMessageHandler(builder.Configuration);

builder.Services.AddHostedService<CheckInWorker>();
builder.Services.AddSingleton<EventStoreClient>(client);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Console.Error.WriteLine(CheckInDB);
    Console.Error.WriteLine(eventSourceConnection);
    app.UseSwagger();
    app.UseSwaggerUI();
}

// auto migrate db
using (var scope = app.Services.CreateScope())
{
    Console.WriteLine("Start migrations Checkin service.");
    var db = scope.ServiceProvider.GetService<CheckInContextDB>();
    // Will perform a migration when booting up the api.
    db.MigrateDB();
    Console.WriteLine("Ended migrations Checkin service.");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
