using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class NameAttributeToHome : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "Homes",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "string.Empty");

        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "HomeDevices",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "string.Empty");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Name",
            table: "Homes");

        migrationBuilder.DropColumn(
            name: "Name",
            table: "HomeDevices");
    }
}
