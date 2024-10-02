using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class InitialCreation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SystemPermissions", x => x.Id);
                table.ForeignKey(
                    name: "FK_SystemPermissions_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id");
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
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
                table.ForeignKey(
                    name: "FK_Users_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Businesses",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RUT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                BusinessOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Businesses", x => x.Id);
                table.ForeignKey(
                    name: "FK_Businesses_Users_BusinessOwnerId",
                    column: x => x.BusinessOwnerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
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
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModelNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Photos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Devices", x => x.Id);
                table.ForeignKey(
                    name: "FK_Devices_Businesses_BusinessId",
                    column: x => x.BusinessId,
                    principalTable: "Businesses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "HomeMembers",
            columns: table => new
            {
                HomeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomeMembers", x => x.HomeMemberId);
                table.ForeignKey(
                    name: "FK_HomeMembers_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id");
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
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "HomePermissions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HomeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomePermissions", x => x.Id);
                table.ForeignKey(
                    name: "FK_HomePermissions_HomeMembers_HomeMemberId",
                    column: x => x.HomeMemberId,
                    principalTable: "HomeMembers",
                    principalColumn: "HomeMemberId");
            });

        migrationBuilder.CreateTable(
            name: "Notifications",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HomeDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Read = table.Column<bool>(type: "bit", nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HomeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    name: "FK_Notifications_HomeMembers_HomeMemberId",
                    column: x => x.HomeMemberId,
                    principalTable: "HomeMembers",
                    principalColumn: "HomeMemberId");
            });

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
            name: "IX_HomeMembers_HomeId",
            table: "HomeMembers",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_HomeMembers_UserId",
            table: "HomeMembers",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_HomePermissions_HomeMemberId",
            table: "HomePermissions",
            column: "HomeMemberId");

        migrationBuilder.CreateIndex(
            name: "IX_Homes_OwnerId",
            table: "Homes",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_HomeDeviceId",
            table: "Notifications",
            column: "HomeDeviceId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_HomeMemberId",
            table: "Notifications",
            column: "HomeMemberId");

        migrationBuilder.CreateIndex(
            name: "IX_SystemPermissions_RoleId",
            table: "SystemPermissions",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_RoleId",
            table: "Users",
            column: "RoleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "HomePermissions");

        migrationBuilder.DropTable(
            name: "Notifications");

        migrationBuilder.DropTable(
            name: "Sessions");

        migrationBuilder.DropTable(
            name: "SystemPermissions");

        migrationBuilder.DropTable(
            name: "HomeDevices");

        migrationBuilder.DropTable(
            name: "HomeMembers");

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
