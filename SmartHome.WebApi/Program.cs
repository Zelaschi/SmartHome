using Microsoft.EntityFrameworkCore;
using SmartHome.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore.Design;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var services = builder.Services;
var configuration = builder.Configuration;

var SmartHomeConnectionString = configuration.GetConnectionString("SmartHome");
if (string.IsNullOrEmpty(SmartHomeConnectionString))
{
    throw new Exception("Missing SmartHome connection string");
}

services.AddDbContext<SmartHomeEFCoreContext>(options => options.UseSqlServer(SmartHomeConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
