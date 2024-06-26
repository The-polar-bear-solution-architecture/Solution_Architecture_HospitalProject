using AppointmentService.CommandsAndEvents.Commands;
using AppointmentService.Controllers;
using AppointmentService.DB;
using AppointmentService.DB.Repository;
using AppointmentService.DomainServices;
//using AppointmentService.Migrations;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Messages.Configuration;
using RabbitMQ.Messages.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Using Docker DB
builder.Services.AddDbContext<AppointmentServiceContext>(options => options.UseSqlServer("Data Source=sql;Initial Catalog=AppointmentService;User ID=sa;Password=Rick@Sanchez;Trust Server Certificate=True"), ServiceLifetime.Singleton);
builder.Services.AddDbContext<AppointmentReadServiceContext>(options => options.UseSqlServer("Data Source=sql;Initial Catalog=AppointmentServiceRead;User ID=sa;Password=Rick@Sanchez;Trust Server Certificate=True"), ServiceLifetime.Singleton);

// Using local DB
//builder.Services.AddDbContext<AppointmentServiceContext>(options => options.UseSqlServer("Data Source =.; Initial Catalog = AppointmentService; Integrated Security = True; Encrypt = False; Trust Server Certificate=True"), ServiceLifetime.Singleton);

//Data Source =.; Initial Catalog = AppointmentService; Integrated Security = True; Encrypt = False; Trust Server Certificate=True
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IAppointmentRepository, EFAppointmentRepository>();
builder.Services.AddTransient<IPhysicianRepository, EFPhysicianRepository>();
builder.Services.AddTransient<IPatientRepository, EFPatientRepository>();
builder.Services.AddTransient<IGeneralPractitionerRepository, EFGeneralPractitionerRepository>();
builder.Services.AddTransient<AppointmentCommandHandler, AppointmentCommandHandler>();
builder.Services.AddTransient<PatientCommandHandler, PatientCommandHandler>();

builder.Services.UseRabbitMQMessagePublisher(builder.Configuration);
builder.Services.UseRabbitMQMessageHandler(builder.Configuration);
builder.Services.AddHostedService<AppointmentWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    Console.WriteLine("Start migrations.");
    var db = scope.ServiceProvider.GetService<AppointmentServiceContext>();
    var readDB = scope.ServiceProvider.GetService<AppointmentReadServiceContext>();
    // Will perform a migration when booting up the api.
    db.MigrateDB();
    readDB.MigrateDB();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


