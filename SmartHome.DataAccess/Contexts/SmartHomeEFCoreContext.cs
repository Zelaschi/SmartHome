using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.InitialSeedData;

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
        ConfigSeedData(modelBuilder);
    }

    private void ConfigSeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = SeedDataConstants.ADMIN_ROLE_ID,
                Name = "Admin"
            },
            new Role
            {
                Id = SeedDataConstants.HOME_OWNER_ROLE_ID,
                Name = "HomeOwner"
            },
            new Role
            {
                Id = SeedDataConstants.BUSINESS_OWNER_ROLE_ID,
                Name = "BusinessOwner"
            });
        modelBuilder.Entity<SystemPermission>().HasData(
            new SystemPermission
            {
                Id = SeedDataConstants.CREATE_OR_DELETE_ADMIN_ACCOUNT_PERMISSION_ID,
                Name = "Create or delete admin account",
                Description = "Create or delete admin account"
            },
            new SystemPermission
            {
                Id = SeedDataConstants.CREATE_BUSINESS_OWNER_ACCOUNT_PERMISSION_ID,
                Name = "Create business owner account",
                Description = "Create business owner account"
            },
            new SystemPermission
            {
                Id = SeedDataConstants.LIST_ALL_ACCOUNTS_PERMISSION_ID,
                Name = "List all accounts",
                Description = "List all accounts"
            },
            new SystemPermission
            {
                Id = SeedDataConstants.LIST_ALL_BUSINESSES_PERMISSION_ID,
                Name = "List all businesses",
                Description = "List all businesses"
            },
            new SystemPermission
            {
                Id = SeedDataConstants.CREATE_BUSINESS_PERMISSION_ID,
                Name = "Create business",
                Description = "Create business"
            },
            new SystemPermission
            {
                Id = SeedDataConstants.CREATE_DEVICE_PERMISSION_ID,
                Name = "Create device",
                Description = "Create device"
            },
            new SystemPermission
            {
                Id = SeedDataConstants.LIST_ALL_DEVICES_PERMISSION_ID,
                Name = "List all devices",
                Description = "List all devices"
            },
            new SystemPermission
            {
                Id = SeedDataConstants.LIST_ALL_DEVICES_TYPES_PERMISSION_ID,
                Name = "List all device types",
                Description = "List all device types"
            },
            new SystemPermission
            {
                Id = SeedDataConstants.CREATE_HOME_PERMISSION_ID,
                Name = "Create home",
                Description = "Create home"
            },
            new SystemPermission
            {
                Id = SeedDataConstants.ADD_MEMBER_TO_HOME_PERMISSION_ID,
                Name = "Add member to home",
                Description = "Add member to home"
            });
        modelBuilder.Entity<RoleSystemPermission>().HasData(
            new RoleSystemPermission
            {
                RoleId = SeedDataConstants.ADMIN_ROLE_ID,
                SystemPermissionId = SeedDataConstants.CREATE_OR_DELETE_ADMIN_ACCOUNT_PERMISSION_ID
            },
            new RoleSystemPermission
            {
                RoleId = SeedDataConstants.ADMIN_ROLE_ID,
                SystemPermissionId = SeedDataConstants.CREATE_BUSINESS_OWNER_ACCOUNT_PERMISSION_ID
            },
            new RoleSystemPermission
            {
                RoleId = SeedDataConstants.ADMIN_ROLE_ID,
                SystemPermissionId = SeedDataConstants.LIST_ALL_ACCOUNTS_PERMISSION_ID
            },
            new RoleSystemPermission
            {
                RoleId = SeedDataConstants.ADMIN_ROLE_ID,
                SystemPermissionId = SeedDataConstants.LIST_ALL_BUSINESSES_PERMISSION_ID
            },
            new RoleSystemPermission
            {
                RoleId = SeedDataConstants.BUSINESS_OWNER_ROLE_ID,
                SystemPermissionId = SeedDataConstants.CREATE_BUSINESS_PERMISSION_ID
            },
            new RoleSystemPermission
            {
                RoleId = SeedDataConstants.BUSINESS_OWNER_ROLE_ID,
                SystemPermissionId = SeedDataConstants.CREATE_DEVICE_PERMISSION_ID
            },
            new RoleSystemPermission
            {
                RoleId = SeedDataConstants.HOME_OWNER_ROLE_ID,
                SystemPermissionId = SeedDataConstants.LIST_ALL_DEVICES_PERMISSION_ID
            },
            new RoleSystemPermission
            {
                RoleId = SeedDataConstants.HOME_OWNER_ROLE_ID,
                SystemPermissionId = SeedDataConstants.LIST_ALL_DEVICES_TYPES_PERMISSION_ID
            },
            new RoleSystemPermission
            {
                RoleId = SeedDataConstants.HOME_OWNER_ROLE_ID,
                SystemPermissionId = SeedDataConstants.CREATE_HOME_PERMISSION_ID
            },
            new RoleSystemPermission
            {
                RoleId = SeedDataConstants.HOME_OWNER_ROLE_ID,
                SystemPermissionId = SeedDataConstants.ADD_MEMBER_TO_HOME_PERMISSION_ID
            }
        );
        modelBuilder.Entity<HomePermission>().HasData(
            new HomePermission
            {
                Id = SeedDataConstants.ADD_MEMBER_TO_HOME_HOMEPERMISSION_ID,
                Name = "Add member to home permission"
            },
            new HomePermission
            {
                Id = SeedDataConstants.ADD_DEVICES_TO_HOME_HOMEPERMISSION_ID,
                Name = "Add devices to home permission"
            },
            new HomePermission
            {
                Id = SeedDataConstants.LIST_DEVICES_HOMEPERMISSION_ID,
                Name = "List home's devices permission"
            },
            new HomePermission
            {
                Id = SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID,
                Name = "Recieve device's notifications"
            }
        );
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = SeedDataConstants.FIRST_ADMIN_ID,
                Name = "First admin",
                Surname = "admin surname",
                Password = "Password@1234",
                Email = "admin1234@gmail.com",
                CreationDate = DateTime.Now,
                RoleId = SeedDataConstants.ADMIN_ROLE_ID
            }
         );
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
