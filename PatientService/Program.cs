using EventStore.Client;
using Microsoft.EntityFrameworkCore;
using PatientService.Domain;
using PatientService.DomainServices;
using PatientService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IGeneralPractitionerRepository, GeneralPractitionerRepository>();
builder.Services.AddDbContext<PatientDBContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Braphia_PatientService")));
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
