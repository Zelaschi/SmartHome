using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class ModelValidatorTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "ValidatorId",
            table: "Businesses",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Validators",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Validators", x => x.Id);
            });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("80e909fb-3c8a-423d-bd46-edde4f85fbe3"),
            column: "CreationDate",
            value: new DateTime(2024, 11, 12, 15, 40, 24, 339, DateTimeKind.Local).AddTicks(8586));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Validators");

        migrationBuilder.DropColumn(
            name: "ValidatorId",
            table: "Businesses");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("80e909fb-3c8a-423d-bd46-edde4f85fbe3"),
            column: "CreationDate",
            value: new DateTime(2024, 11, 11, 17, 51, 34, 850, DateTimeKind.Local).AddTicks(8013));
    }
}
