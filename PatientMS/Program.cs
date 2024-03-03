using Microsoft.EntityFrameworkCore;
using PatientMS.PatientCore.Interfaces;
using PatientMS.PatientInfrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database connection
var connectionString = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add interface
builder.Services.AddScoped<IPatientInterface, PatientInterface>();

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
