using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class RoomsCreation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Photos",
            table: "Devices");

        migrationBuilder.AddColumn<Guid>(
            name: "RoomId",
            table: "HomeDevices",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Photo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Photo", x => x.Id);
                table.ForeignKey(
                    name: "FK_Photo_Devices_DeviceId",
                    column: x => x.DeviceId,
                    principalTable: "Devices",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Room",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Room", x => x.Id);
                table.ForeignKey(
                    name: "FK_Room_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_HomeDevices_RoomId",
            table: "HomeDevices",
            column: "RoomId");

        migrationBuilder.CreateIndex(
            name: "IX_Photo_DeviceId",
            table: "Photo",
            column: "DeviceId");

        migrationBuilder.CreateIndex(
            name: "IX_Room_HomeId",
            table: "Room",
            column: "HomeId");

        migrationBuilder.AddForeignKey(
            name: "FK_HomeDevices_Room_RoomId",
            table: "HomeDevices",
            column: "RoomId",
            principalTable: "Room",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_HomeDevices_Room_RoomId",
            table: "HomeDevices");

        migrationBuilder.DropTable(
            name: "Photo");

        migrationBuilder.DropTable(
            name: "Room");

        migrationBuilder.DropIndex(
            name: "IX_HomeDevices_RoomId",
            table: "HomeDevices");

        migrationBuilder.DropColumn(
            name: "RoomId",
            table: "HomeDevices");

        migrationBuilder.AddColumn<string>(
            name: "Photos",
            table: "Devices",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);
    }
}
