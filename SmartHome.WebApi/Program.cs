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

var SmartHomeConnectionString = configuration.GetConnectionString("SmartHome");
if (string.IsNullOrEmpty(SmartHomeConnectionString))
{
    throw new Exception("Missing SmartHome connection string");
}

services.AddDbContext<SmartHomeEFCoreContext>(options => options.UseSqlServer(SmartHomeConnectionString));

services.AddScoped<IBusinessesLogic, BusinessService>();
services.AddScoped<IDeviceLogic, DeviceService>();
services.AddScoped<ISecurityCameraLogic, DeviceService>();
services.AddScoped<IHomeOwnerLogic, UserService>();
services.AddScoped<IUsersLogic, UserService>();
services.AddScoped<IBusinessOwnerLogic, UserService>();
services.AddScoped<ILoginLogic, SessionService>();
services.AddScoped<IRoleLogic, RoleService>();
services.AddScoped<IHomeLogic, HomeService>();

services.AddScoped<IGenericRepository<Business>, BusinessRepository>();
services.AddScoped<IGenericRepository<Device>, DeviceRepository>();
services.AddScoped<IGenericRepository<Role>, RoleRepository>();
services.AddScoped<IGenericRepository<Session>, SessionRepository>();
services.AddScoped<IGenericRepository<User>, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
