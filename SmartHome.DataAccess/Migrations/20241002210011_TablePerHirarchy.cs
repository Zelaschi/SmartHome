using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class TablePerHirarchy : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "DeviceType",
            table: "Devices",
            type: "nvarchar(21)",
            maxLength: 21,
            nullable: false,
            defaultValue: "Window Sensor");

        migrationBuilder.AddColumn<bool>(
            name: "Indoor",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "MovementDetection",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "Outdoor",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "PersonDetection",
            table: "Devices",
            type: "bit",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "DeviceType",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "Indoor",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "MovementDetection",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "Outdoor",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "PersonDetection",
            table: "Devices");
    }
}
