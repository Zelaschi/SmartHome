﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHome.DataAccess.Contexts;

#nullable disable

namespace SmartHome.DataAccess.Migrations
{
    [DbContext(typeof(SmartHomeEFCoreContext))]
    partial class SmartHomeEFCoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Business", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessOwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RUT")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessOwnerId");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceType")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("ModelNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.ToTable("Devices", (string)null);

                    b.HasDiscriminator<string>("DeviceType").HasValue("Window Sensor");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Home", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DoorNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Latitude")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Longitude")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainStreet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxMembers")
                        .HasColumnType("int");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Homes");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.HomeDevice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Online")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("HomeId");

                    b.ToTable("HomeDevices");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.HomeMember", b =>
                {
                    b.Property<Guid>("HomeMemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HomeMemberId");

                    b.HasIndex("HomeId");

                    b.HasIndex("UserId");

                    b.ToTable("HomeMembers");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.HomeMemberPermission", b =>
                {
                    b.Property<Guid>("HomeMemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HomeMemberId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("HomeMemberPermission");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.HomePermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HomePermissions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("98bb8133-688a-4f1d-8587-87c485df6534"),
                            Name = "Add member to home permission"
                        },
                        new
                        {
                            Id = new Guid("c49f2858-72fc-422d-bd4b-f49b482f80bd"),
                            Name = "Add devices to home permission"
                        },
                        new
                        {
                            Id = new Guid("fa0cad23-153b-46b5-a690-91d0d7677c31"),
                            Name = "List home's devices permission"
                        },
                        new
                        {
                            Id = new Guid("9d7f6847-e8d5-4515-b9ac-0f0c00fcc7b3"),
                            Name = "Recieve device's notifications"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HomeDeviceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("HomeMemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Read")
                        .HasColumnType("bit");

                    b.Property<string>("Time")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HomeDeviceId");

                    b.HasIndex("HomeMemberId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"),
                            Name = "Admin"
                        },
                        new
                        {
                            Id = new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"),
                            Name = "HomeOwner"
                        },
                        new
                        {
                            Id = new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"),
                            Name = "BusinessOwner"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.RoleSystemPermission", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SystemPermissionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "SystemPermissionId");

                    b.HasIndex("SystemPermissionId");

                    b.ToTable("RoleSystemPermission");

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"),
                            SystemPermissionId = new Guid("f22942d7-9bc0-4458-a713-15c9010deaa1")
                        },
                        new
                        {
                            RoleId = new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"),
                            SystemPermissionId = new Guid("b3eef741-8d56-4263-a633-7e176981feec")
                        },
                        new
                        {
                            RoleId = new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"),
                            SystemPermissionId = new Guid("b6f31ea3-aca9-4757-9905-eff4ef100564")
                        },
                        new
                        {
                            RoleId = new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"),
                            SystemPermissionId = new Guid("4e2851c0-c5bb-4e52-b6a5-badadbbd83dd")
                        },
                        new
                        {
                            RoleId = new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"),
                            SystemPermissionId = new Guid("f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95")
                        },
                        new
                        {
                            RoleId = new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"),
                            SystemPermissionId = new Guid("7c1d3527-e47c-43ac-b979-447a05558f25")
                        },
                        new
                        {
                            RoleId = new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"),
                            SystemPermissionId = new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73")
                        },
                        new
                        {
                            RoleId = new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"),
                            SystemPermissionId = new Guid("17133752-d60e-4f35-916f-6651ab4463e4")
                        },
                        new
                        {
                            RoleId = new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"),
                            SystemPermissionId = new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360")
                        },
                        new
                        {
                            RoleId = new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"),
                            SystemPermissionId = new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b")
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Session", b =>
                {
                    b.Property<Guid>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SessionId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.SystemPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SystemPermissions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f22942d7-9bc0-4458-a713-15c9010deaa1"),
                            Description = "Create or delete admin account",
                            Name = "Create or delete admin account"
                        },
                        new
                        {
                            Id = new Guid("b3eef741-8d56-4263-a633-7e176981feec"),
                            Description = "Create business owner account",
                            Name = "Create business owner account"
                        },
                        new
                        {
                            Id = new Guid("b6f31ea3-aca9-4757-9905-eff4ef100564"),
                            Description = "List all accounts",
                            Name = "List all accounts"
                        },
                        new
                        {
                            Id = new Guid("4e2851c0-c5bb-4e52-b6a5-badadbbd83dd"),
                            Description = "List all businesses",
                            Name = "List all businesses"
                        },
                        new
                        {
                            Id = new Guid("f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95"),
                            Description = "Create business",
                            Name = "Create business"
                        },
                        new
                        {
                            Id = new Guid("7c1d3527-e47c-43ac-b979-447a05558f25"),
                            Description = "Create device",
                            Name = "Create device"
                        },
                        new
                        {
                            Id = new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73"),
                            Description = "List all devices",
                            Name = "List all devices"
                        },
                        new
                        {
                            Id = new Guid("17133752-d60e-4f35-916f-6651ab4463e4"),
                            Description = "List all device types",
                            Name = "List all device types"
                        },
                        new
                        {
                            Id = new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360"),
                            Description = "Create home",
                            Name = "Create home"
                        },
                        new
                        {
                            Id = new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b"),
                            Description = "Add member to home",
                            Name = "Add member to home"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.User", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("Complete")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePhoto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("80e909fb-3c8a-423d-bd46-edde4f85fbe3"),
                            Email = "admin1234@gmail.com",
                            Name = "First admin",
                            Password = "Password@1234",
                            RoleId = new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"),
                            Surname = "admin surname"
                        });
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.SecurityCamera", b =>
                {
                    b.HasBaseType("SmartHome.BusinessLogic.Domain.Device");

                    b.Property<bool>("Indoor")
                        .HasColumnType("bit");

                    b.Property<bool>("MovementDetection")
                        .HasColumnType("bit");

                    b.Property<bool>("Outdoor")
                        .HasColumnType("bit");

                    b.Property<bool>("PersonDetection")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("Security Camera");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Business", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Domain.User", "BusinessOwner")
                        .WithMany()
                        .HasForeignKey("BusinessOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusinessOwner");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Device", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Domain.Business", "Business")
                        .WithMany()
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Home", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Domain.User", "Owner")
                        .WithMany("Houses")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.HomeDevice", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Domain.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Domain.Home", null)
                        .WithMany("Devices")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.HomeMember", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Domain.Home", null)
                        .WithMany("Members")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.HomeMemberPermission", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Domain.HomeMember", "HomeMember")
                        .WithMany()
                        .HasForeignKey("HomeMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Domain.HomePermission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HomeMember");

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Notification", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Domain.HomeDevice", "HomeDevice")
                        .WithMany()
                        .HasForeignKey("HomeDeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Domain.HomeMember", null)
                        .WithMany("Notifications")
                        .HasForeignKey("HomeMemberId");

                    b.Navigation("HomeDevice");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.RoleSystemPermission", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Domain.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.BusinessLogic.Domain.SystemPermission", "SystemPermission")
                        .WithMany()
                        .HasForeignKey("SystemPermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("SystemPermission");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.User", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Domain.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Home", b =>
                {
                    b.Navigation("Devices");

                    b.Navigation("Members");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.HomeMember", b =>
                {
                    b.Navigation("Notifications");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.User", b =>
                {
                    b.Navigation("Houses");
                });
#pragma warning restore 612, 618
        }
    }
}
