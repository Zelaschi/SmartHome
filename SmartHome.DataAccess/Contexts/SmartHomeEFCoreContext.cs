using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

    public SmartHomeEFCoreContext(DbContextOptions<SmartHomeEFCoreContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigSchema(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void ConfigSchema(ModelBuilder modelBuilder)
    {
         modelBuilder.Entity<Role>()
            .HasMany(r => r.SystemPermissions)
            .WithMany(s => s.Roles)
           .UsingEntity<RoleSystemPermission>(
           r => r.HasOne(x => x.SystemPermission).WithMany().HasForeignKey(x => x.SystemPermissionId),
           l => l.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId));

        modelBuilder.Entity<HomeMember>()
            .HasMany(h => h.HomePermissions)
            .WithMany(m => m.HomeMembers)
            .UsingEntity<HomeMemberPermission>(
            r => r.HasOne(x => x.Permission).WithMany().HasForeignKey(x => x.PermissionId),
            l => l.HasOne(x => x.HomeMember).WithMany().HasForeignKey(x => x.HomeMemberId));

        modelBuilder.Entity<Device>()
            .ToTable("Devices")
            .HasDiscriminator<string>("DeviceType")
            .HasValue<Device>("Window Sensor")
            .HasValue<SecurityCamera>("Security Camera");

        modelBuilder.Entity<Home>()
            .HasMany(h => h.Members)
            .WithOne()
            .HasForeignKey(hm => hm.HomeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Home>()
            .HasMany(h => h.Devices)
            .WithOne()
            .HasForeignKey(hd => hd.HomeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
