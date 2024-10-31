using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DevicesTypesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOn",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Open",
                table: "Devices");

            migrationBuilder.AddColumn<bool>(
                name: "IsOn",
                table: "HomeDevices",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "HomeDevices",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOn",
                table: "HomeDevices");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "HomeDevices");

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
    }
}
