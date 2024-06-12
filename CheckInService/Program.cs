using CheckInService.CommandHandlers;
using CheckInService.Controllers;
using CheckInService.DBContexts;
using CheckInService.Repositories;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Messages.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string CheckInDB = builder.Configuration.GetConnectionString("CheckInDB");
//-|| Regular database || Configuration
builder.Services.AddDbContext<CheckInContextDB>(options => options.UseSqlServer(CheckInDB));

// Service via dependency injection
builder.Services.AddScoped<CheckInRepository, CheckInRepository>();
builder.Services.AddScoped<AppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<PhysicianRepo, PhysicianRepo>();
builder.Services.AddScoped<PatientRepo, PatientRepo>();
builder.Services.AddScoped<CheckInCommandHandler, CheckInCommandHandler>();

// Use rabbitMQ Publisher
builder.Services.UseRabbitMQMessagePublisher(builder.Configuration);

builder.Services.UseRabbitMQMessageHandler(builder.Configuration);
builder.Services.AddHostedService<CheckInWorker>();

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
