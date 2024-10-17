using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class NewDevicesTypes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsOn",
            table: "Devices",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "Open",
            table: "Devices",
            type: "bit",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsOn",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "Open",
            table: "Devices");
    }
}
