using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.DataAccess.Contexts;
public sealed class SmartHomeEFCoreContext : DbContext
{
    public DbSet<Home> Homes { get; set; }
    public DbSet<HomeMember> HomeMembers { get; set; }
    public DbSet<HomePermission> HomePermissions { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<SystemPermission> SystemPermissions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Business> Businesses { get; set; }
    public DbSet<HomeDevice> HomeDevices { get; set; }
    public DbSet<Device> Devices { get; set; }

    public SmartHomeEFCoreContext(DbContextOptions options)
        : base(options)
    {
    }
}
