using AppointmentService.DB;
using AppointmentService.DB.Repository;
using AppointmentService.DomainServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppointmentServiceContext>(options => options.UseSqlServer("Data Source=.;Initial Catalog=AppointmentService;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAppointmentRepository, EFAppointmentRepository>();
builder.Services.AddScoped<IPhysicianRepository, EFPhysicianRepository>();
builder.Services.AddScoped<IPatientRepository, EFPatientRepository>();

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
