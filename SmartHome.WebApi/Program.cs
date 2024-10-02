using Microsoft.EntityFrameworkCore;
using SmartHome.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore.Design;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Services;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Repositories;
using SmartHome.BusinessLogic.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var services = builder.Services;
var configuration = builder.Configuration;

var smartHomeConnectionString = configuration.GetConnectionString("SmartHome");
if (string.IsNullOrEmpty(smartHomeConnectionString))
{
    throw new Exception("Missing SmartHome connection string");
}

services.AddDbContext<SmartHomeEFCoreContext>(options => options.UseSqlServer(smartHomeConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
