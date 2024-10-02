﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHome.DataAccess.Contexts;

#nullable disable

namespace SmartHome.DataAccess.Migrations
{
    [DbContext(typeof(SmartHomeEFCoreContext))]
    [Migration("20241002210337_TablePerHierarchy")]
    partial class TablePerHierarchy
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("HomeMemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HomeMemberId");

                    b.ToTable("HomePermissions");
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

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.RoleSystemPermission", b =>
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

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("SystemPermissions");
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

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
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
                    b.HasOne("SmartHome.BusinessLogic.Domain.HomeMember", null)
                        .WithMany("HomePermissions")
                        .HasForeignKey("HomeMemberId");
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
                    b.HasOne("SmartHome.BusinessLogic.Domain.Role", null)
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.User", b =>
                {
                    b.HasOne("SmartHome.BusinessLogic.Domain.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Home", b =>
                {
                    b.Navigation("Devices");

                    b.Navigation("Members");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.HomeMember", b =>
                {
                    b.Navigation("HomePermissions");

                    b.Navigation("Notifications");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.Role", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("SmartHome.BusinessLogic.Domain.User", b =>
                {
                    b.Navigation("Houses");
                });
#pragma warning restore 612, 618
        }
    }
}
