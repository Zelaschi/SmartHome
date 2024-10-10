using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class FirstMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "HomePermissions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomePermissions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Sessions",
            columns: table => new
            {
                SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Sessions", x => x.SessionId);
            });

        migrationBuilder.CreateTable(
            name: "SystemPermissions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SystemPermissions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Complete = table.Column<bool>(type: "bit", nullable: true),
                ProfilePhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
                table.ForeignKey(
                    name: "FK_Users_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RoleSystemPermission",
            columns: table => new
            {
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SystemPermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleSystemPermission", x => new { x.RoleId, x.SystemPermissionId });
                table.ForeignKey(
                    name: "FK_RoleSystemPermission_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RoleSystemPermission_SystemPermissions_SystemPermissionId",
                    column: x => x.SystemPermissionId,
                    principalTable: "SystemPermissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Businesses",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RUT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                BusinessOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Businesses", x => x.Id);
                table.ForeignKey(
                    name: "FK_Businesses_Users_BusinessOwnerId",
                    column: x => x.BusinessOwnerId,
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Homes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MainStreet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DoorNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Latitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Longitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                MaxMembers = table.Column<int>(type: "int", nullable: false),
                OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Homes", x => x.Id);
                table.ForeignKey(
                    name: "FK_Homes_Users_OwnerId",
                    column: x => x.OwnerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Devices",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Type = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                ModelNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Photos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Outdoor = table.Column<bool>(type: "bit", nullable: true),
                Indoor = table.Column<bool>(type: "bit", nullable: true),
                MovementDetection = table.Column<bool>(type: "bit", nullable: true),
                PersonDetection = table.Column<bool>(type: "bit", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Devices", x => x.Id);
                table.ForeignKey(
                    name: "FK_Devices_Businesses_BusinessId",
                    column: x => x.BusinessId,
                    principalTable: "Businesses",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "HomeMembers",
            columns: table => new
            {
                HomeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomeMembers", x => x.HomeMemberId);
                table.ForeignKey(
                    name: "FK_HomeMembers_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_HomeMembers_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "HomeDevices",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Online = table.Column<bool>(type: "bit", nullable: false),
                DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomeDevices", x => x.Id);
                table.ForeignKey(
                    name: "FK_HomeDevices_Devices_DeviceId",
                    column: x => x.DeviceId,
                    principalTable: "Devices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_HomeDevices_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "HomeMemberPermission",
            columns: table => new
            {
                HomeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomeMemberPermission", x => new { x.HomeMemberId, x.PermissionId });
                table.ForeignKey(
                    name: "FK_HomeMemberPermission_HomeMembers_HomeMemberId",
                    column: x => x.HomeMemberId,
                    principalTable: "HomeMembers",
                    principalColumn: "HomeMemberId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_HomeMemberPermission_HomePermissions_PermissionId",
                    column: x => x.PermissionId,
                    principalTable: "HomePermissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Notifications",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HomeDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                DetectedPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notifications", x => x.Id);
                table.ForeignKey(
                    name: "FK_Notifications_HomeDevices_HomeDeviceId",
                    column: x => x.HomeDeviceId,
                    principalTable: "HomeDevices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Notifications_Users_DetectedPersonId",
                    column: x => x.DetectedPersonId,
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "HomeMemberNotification",
            columns: table => new
            {
                NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HomeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Read = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomeMemberNotification", x => new { x.HomeMemberId, x.NotificationId });
                table.ForeignKey(
                    name: "FK_HomeMemberNotification_HomeMembers_HomeMemberId",
                    column: x => x.HomeMemberId,
                    principalTable: "HomeMembers",
                    principalColumn: "HomeMemberId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_HomeMemberNotification_Notifications_NotificationId",
                    column: x => x.NotificationId,
                    principalTable: "Notifications",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.InsertData(
            table: "HomePermissions",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                { new Guid("98bb8133-688a-4f1d-8587-87c485df6534"), "Add member to home permission" },
                { new Guid("9d7f6847-e8d5-4515-b9ac-0f0c00fcc7b3"), "Recieve device's notifications" },
                { new Guid("c49f2858-72fc-422d-bd4b-f49b482f80bd"), "Add devices to home permission" },
                { new Guid("fa0cad23-153b-46b5-a690-91d0d7677c31"), "List home's devices permission" }
            });

        migrationBuilder.InsertData(
            table: "Roles",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                { new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"), "BusinessOwner" },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), "HomeOwner" },
                { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), "Admin" }
            });

        migrationBuilder.InsertData(
            table: "SystemPermissions",
            columns: new[] { "Id", "Description", "Name" },
            values: new object[,]
            {
                { new Guid("0957ea49-6c58-4f9a-9cbc-bb4a300077f3"), "List all users homes", "List all users homes" },
                { new Guid("17133752-d60e-4f35-916f-6651ab4463e4"), "List all device types", "List all device types" },
                { new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360"), "Create home", "Create home" },
                { new Guid("48644f45-4d83-4961-924b-7733881258e9"), "Lets a user with the role homeOwner access the home related endpoints", "Home related permission" },
                { new Guid("4e2851c0-c5bb-4e52-b6a5-badadbbd83dd"), "List all businesses", "List all businesses" },
                { new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b"), "Add member to home", "Add member to home" },
                { new Guid("7c1d3527-e47c-43ac-b979-447a05558f25"), "Create device", "Create device" },
                { new Guid("b3eef741-8d56-4263-a633-7e176981feec"), "Create business owner account", "Create business owner account" },
                { new Guid("b404659a-35a4-4486-867f-db4c24f9f827"), "Create notification", "Create notification" },
                { new Guid("b6f31ea3-aca9-4757-9905-eff4ef100564"), "List all accounts", "List all accounts" },
                { new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73"), "List all devices", "List all devices" },
                { new Guid("dde0bac9-e646-4d9f-96f5-c77e7295cb4b"), "Lets a user access all its notifications", "List all users notifications" },
                { new Guid("f22942d7-9bc0-4458-a713-15c9010deaa1"), "Create or delete admin account", "Create or delete admin account" },
                { new Guid("f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95"), "Create business", "Create business" }
            });

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleId", "SystemPermissionId" },
            values: new object[,]
            {
                { new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"), new Guid("7c1d3527-e47c-43ac-b979-447a05558f25") },
                { new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"), new Guid("f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("0957ea49-6c58-4f9a-9cbc-bb4a300077f3") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("17133752-d60e-4f35-916f-6651ab4463e4") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("48644f45-4d83-4961-924b-7733881258e9") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("b404659a-35a4-4486-867f-db4c24f9f827") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("dde0bac9-e646-4d9f-96f5-c77e7295cb4b") },
                { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("4e2851c0-c5bb-4e52-b6a5-badadbbd83dd") },
                { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("b3eef741-8d56-4263-a633-7e176981feec") },
                { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("b6f31ea3-aca9-4757-9905-eff4ef100564") },
                { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("f22942d7-9bc0-4458-a713-15c9010deaa1") }
            });

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "Complete", "Email", "Name", "Password", "ProfilePhoto", "RoleId", "Surname" },
            values: new object[] { new Guid("80e909fb-3c8a-423d-bd46-edde4f85fbe3"), null, "admin1234@gmail.com", "First admin", "Password@1234", null, new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), "admin surname" });

        migrationBuilder.CreateIndex(
            name: "IX_Businesses_BusinessOwnerId",
            table: "Businesses",
            column: "BusinessOwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_Devices_BusinessId",
            table: "Devices",
            column: "BusinessId");

        migrationBuilder.CreateIndex(
            name: "IX_HomeDevices_DeviceId",
            table: "HomeDevices",
            column: "DeviceId");

        migrationBuilder.CreateIndex(
            name: "IX_HomeDevices_HomeId",
            table: "HomeDevices",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_HomeMemberNotification_NotificationId",
            table: "HomeMemberNotification",
            column: "NotificationId");

        migrationBuilder.CreateIndex(
            name: "IX_HomeMemberPermission_PermissionId",
            table: "HomeMemberPermission",
            column: "PermissionId");

        migrationBuilder.CreateIndex(
            name: "IX_HomeMembers_HomeId",
            table: "HomeMembers",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_HomeMembers_UserId",
            table: "HomeMembers",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Homes_OwnerId",
            table: "Homes",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_DetectedPersonId",
            table: "Notifications",
            column: "DetectedPersonId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_HomeDeviceId",
            table: "Notifications",
            column: "HomeDeviceId");

        migrationBuilder.CreateIndex(
            name: "IX_RoleSystemPermission_SystemPermissionId",
            table: "RoleSystemPermission",
            column: "SystemPermissionId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_RoleId",
            table: "Users",
            column: "RoleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "HomeMemberNotification");

        migrationBuilder.DropTable(
            name: "HomeMemberPermission");

        migrationBuilder.DropTable(
            name: "RoleSystemPermission");

        migrationBuilder.DropTable(
            name: "Sessions");

        migrationBuilder.DropTable(
            name: "Notifications");

        migrationBuilder.DropTable(
            name: "HomeMembers");

        migrationBuilder.DropTable(
            name: "HomePermissions");

        migrationBuilder.DropTable(
            name: "SystemPermissions");

        migrationBuilder.DropTable(
            name: "HomeDevices");

        migrationBuilder.DropTable(
            name: "Devices");

        migrationBuilder.DropTable(
            name: "Homes");

        migrationBuilder.DropTable(
            name: "Businesses");

        migrationBuilder.DropTable(
            name: "Users");

        migrationBuilder.DropTable(
            name: "Roles");
    }
}
