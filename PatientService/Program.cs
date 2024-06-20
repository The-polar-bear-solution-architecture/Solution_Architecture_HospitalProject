using Microsoft.EntityFrameworkCore;
using PatientService.Controllers;
using PatientService.Data;
using PatientService.Domain;
using PatientService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PatientRepository>();
builder.Services.AddDbContext<PatientDBContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("PatientDBContext")));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();

var app = builder.Build();


/*using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PatientDBContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}*/

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
