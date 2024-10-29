using Microsoft.EntityFrameworkCore;
using SmartHome.DataAccess.Contexts;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Services;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.DataAccess.Repositories;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.ExtraRepositoryInterfaces;

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

services.AddScoped<IHomeOwnerLogic, UserService>();
services.AddScoped<IUsersLogic, UserService>();
services.AddScoped<IBusinessOwnerLogic, UserService>();
services.AddScoped<IAdminLogic, UserService>();

services.AddScoped<ILoginLogic, SessionService>();
services.AddScoped<ISessionLogic, SessionService>();

services.AddScoped<ISystemPermissionLogic, RoleService>();
services.AddScoped<IRoleLogic, RoleService>();

services.AddScoped<IBusinessesLogic, BusinessService>();

services.AddScoped<IDeviceLogic, DeviceService>();
services.AddScoped<ISecurityCameraLogic, DeviceService>();
services.AddScoped<ICreateDeviceLogic, DeviceService>();

services.AddScoped<IHomeLogic, HomeService>();
services.AddScoped<IHomePermissionLogic, HomeService>();
services.AddScoped<IHomeMemberLogic, HomeService>();
services.AddScoped<INotificationLogic, HomeService>();

services.AddScoped<IGenericRepository<User>, UserRepository>();
services.AddScoped<IGenericRepository<Session>, SessionRepository>();
services.AddScoped<IGenericRepository<Home>, HomeRepository>();
services.AddScoped<IHomesFromUserRepository, HomeRepository>();
services.AddScoped<IGenericRepository<Role>, RoleRepository>();
services.AddScoped<IGenericRepository<HomePermission>, HomePermissionRepository>();
services.AddScoped<IGenericRepository<Device>, DeviceRepository>();
services.AddScoped<IDeviceTypeRepository, DeviceRepository>();
services.AddScoped<IGenericRepository<Business>, BusinessRepository>();
services.AddScoped<IGenericRepository<HomeDevice>, HomeDeviceRepository>();
services.AddScoped<IGenericRepository<HomeMember>, HomeMemberRepository>();
services.AddScoped<IGenericRepository<SystemPermission>, SystemPermissionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
