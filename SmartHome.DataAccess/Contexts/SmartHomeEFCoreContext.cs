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
    public DbSet<Room> Rooms { get; set; }
    public DbSet<SecurityCamera> SecurityCameras { get; set; }
    public DbSet<ModelNumberValidator> Validators { get; set; }

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
                Id = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID),
                Name = "Admin"
            },
            new Role
            {
                Id = Guid.Parse(SeedDataConstants.HOME_OWNER_ROLE_ID),
                Name = "HomeOwner"
            },
            new Role
            {
                Id = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_ROLE_ID),
                Name = "BusinessOwner"
            },
            new Role
            {
                Id = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                Name = "AdminHomeOwner"
            },
            new Role
            {
                Id = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID),
                Name = "BusinessOwnerHomeOwner"
            });

        modelBuilder.Entity<SystemPermission>().HasData(
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.CREATE_OR_DELETE_ADMIN_ACCOUNT_PERMISSION_ID),
                Name = "Create or delete admin account",
                Description = "Create or delete admin account"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.CREATE_BUSINESS_OWNER_ACCOUNT_PERMISSION_ID),
                Name = "Create business owner account",
                Description = "Create business owner account"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.LIST_ALL_ACCOUNTS_PERMISSION_ID),
                Name = "List all accounts",
                Description = "List all accounts"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.LIST_ALL_BUSINESSES_PERMISSION_ID),
                Name = "List all businesses",
                Description = "List all businesses"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.CREATE_BUSINESS_PERMISSION_ID),
                Name = "Create business",
                Description = "Create business"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.CREATE_DEVICE_PERMISSION_ID),
                Name = "Create device",
                Description = "Create device"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.LIST_ALL_DEVICES_PERMISSION_ID),
                Name = "List all devices",
                Description = "List all devices"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.LIST_ALL_DEVICES_TYPES_PERMISSION_ID),
                Name = "List all device types",
                Description = "List all device types"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.CREATE_HOME_PERMISSION_ID),
                Name = "Create home",
                Description = "Create home"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.ADD_MEMBER_TO_HOME_PERMISSION_ID),
                Name = "Add member to home",
                Description = "Add member to home"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.HOME_RELATED_PERMISSION_ID),
                Name = "Home related permission",
                Description = "Lets a user with the role homeOwner access the home related endpoints"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.LIST_ALL_USERS_HOMES_PERMISSION_ID),
                Name = "List all users homes",
                Description = "List all users homes"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.LIST_ALL_USERS_NOTIFICATIONS_PERMISSION_ID),
                Name = "List all users notifications",
                Description = "Lets a user access all its notifications"
            },
            new SystemPermission
            {
                Id = Guid.Parse(SeedDataConstants.CREATE_NOTIFICATION_PERMISSION_ID),
                Name = "Create notification",
                Description = "Create notification"
            }
            );

        modelBuilder.Entity<RoleSystemPermission>().HasData(
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_NOTIFICATION_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_USERS_NOTIFICATIONS_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_USERS_HOMES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.HOME_RELATED_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_OR_DELETE_ADMIN_ACCOUNT_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_BUSINESS_OWNER_ACCOUNT_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_ACCOUNTS_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_BUSINESSES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_BUSINESS_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_DEVICE_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_DEVICES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_DEVICES_TYPES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_DEVICES_TYPES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_HOME_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.ADD_MEMBER_TO_HOME_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_OR_DELETE_ADMIN_ACCOUNT_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_BUSINESS_OWNER_ACCOUNT_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_ACCOUNTS_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_BUSINESSES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_DEVICES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_DEVICES_TYPES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_HOME_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.ADD_MEMBER_TO_HOME_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_NOTIFICATION_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_USERS_NOTIFICATIONS_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_USERS_HOMES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.HOME_RELATED_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_NOTIFICATION_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_USERS_NOTIFICATIONS_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_USERS_HOMES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.HOME_RELATED_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_BUSINESS_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_DEVICE_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_DEVICES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.LIST_ALL_DEVICES_TYPES_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.CREATE_HOME_PERMISSION_ID)
            },
            new RoleSystemPermission
            {
                RoleId = Guid.Parse(SeedDataConstants.BUSINESS_OWNER_HOME_OWNER_ROLE_ID),
                SystemPermissionId = Guid.Parse(SeedDataConstants.ADD_MEMBER_TO_HOME_PERMISSION_ID)
            }
        );

        modelBuilder.Entity<HomePermission>().HasData(
            new HomePermission
            {
                Id = Guid.Parse(SeedDataConstants.ADD_MEMBER_TO_HOME_HOMEPERMISSION_ID),
                Name = "Add member to home permission"
            },
            new HomePermission
            {
                Id = Guid.Parse(SeedDataConstants.ADD_DEVICES_TO_HOME_HOMEPERMISSION_ID),
                Name = "Add devices to home permission"
            },
            new HomePermission
            {
                Id = Guid.Parse(SeedDataConstants.LIST_DEVICES_HOMEPERMISSION_ID),
                Name = "List home's devices permission"
            },
            new HomePermission
            {
                Id = Guid.Parse(SeedDataConstants.RECIEVE_NOTIFICATIONS_HOMEPERMISSION_ID),
                Name = "Recieve device's notifications"
            },
            new HomePermission
            {
                Id = Guid.Parse(SeedDataConstants.CREATE_ROOM_PERMISSION_ID),
                Name = "Create room permission"
            }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse(SeedDataConstants.FIRST_ADMIN_ID),
                Name = "First admin",
                Surname = "admin surname",
                Password = "Password@1234",
                Email = "admin1234@gmail.com",
                CreationDate = DateTime.Now,
                RoleId = Guid.Parse(SeedDataConstants.ADMIN_ROLE_ID)
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

        modelBuilder.Entity<HomeMember>()
            .HasMany(n => n.Notifications)
            .WithMany(m => m.HomeMembers)
            .UsingEntity<HomeMemberNotification>(
               r => r.HasOne(x => x.Notification)
              .WithMany(n => n.HomeMemberNotifications)
              .HasForeignKey(x => x.NotificationId),
               l => l.HasOne(x => x.HomeMember)
              .WithMany(m => m.HomeMemberNotifications)
              .HasForeignKey(x => x.HomeMemberId));

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

        modelBuilder.Entity<SecurityCamera>()
            .HasBaseType<Device>()
        .Property(c => c.Outdoor)
        .HasColumnName("Outdoor");

        modelBuilder.Entity<SecurityCamera>()
            .HasBaseType<Device>()
            .Property(c => c.Indoor)
            .HasColumnName("Indoor");

        modelBuilder.Entity<SecurityCamera>()
            .HasBaseType<Device>()
            .Property(c => c.MovementDetection)
            .HasColumnName("MovementDetection");

        modelBuilder.Entity<SecurityCamera>()
            .HasBaseType<Device>()
            .Property(c => c.PersonDetection)
            .HasColumnName("PersonDetection");

        modelBuilder.Entity<HomeMemberNotification>()
            .HasOne(hmn => hmn.HomeMember)
            .WithMany(hm => hm.HomeMemberNotifications)
            .HasForeignKey(hmn => hmn.HomeMemberId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HomeMemberNotification>()
            .HasOne(hmn => hmn.Notification)
            .WithMany(n => n.HomeMemberNotifications)
            .HasForeignKey(hmn => hmn.NotificationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
