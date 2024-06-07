using CheckInService.DBContexts;
using CheckInService.Repositories;
using Microsoft.EntityFrameworkCore;

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
