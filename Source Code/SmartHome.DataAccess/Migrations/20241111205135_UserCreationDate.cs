using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class UserCreationDate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_HomeDevices_Room_RoomId",
            table: "HomeDevices");

        migrationBuilder.DropForeignKey(
            name: "FK_Room_Homes_HomeId",
            table: "Room");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Room",
            table: "Room");

        migrationBuilder.RenameTable(
            name: "Room",
            newName: "Rooms");

        migrationBuilder.RenameIndex(
            name: "IX_Room_HomeId",
            table: "Rooms",
            newName: "IX_Rooms_HomeId");

        migrationBuilder.AddColumn<DateTime>(
            name: "CreationDate",
            table: "Users",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AlterColumn<string>(
            name: "Type",
            table: "Devices",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Rooms",
            table: "Rooms",
            column: "Id");

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleId", "SystemPermissionId" },
            values: new object[] { new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"), new Guid("17133752-d60e-4f35-916f-6651ab4463e4") });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("80e909fb-3c8a-423d-bd46-edde4f85fbe3"),
            column: "CreationDate",
            value: new DateTime(2024, 11, 11, 17, 51, 34, 850, DateTimeKind.Local).AddTicks(8013));

        migrationBuilder.AddForeignKey(
            name: "FK_HomeDevices_Rooms_RoomId",
            table: "HomeDevices",
            column: "RoomId",
            principalTable: "Rooms",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Rooms_Homes_HomeId",
            table: "Rooms",
            column: "HomeId",
            principalTable: "Homes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_HomeDevices_Rooms_RoomId",
            table: "HomeDevices");

        migrationBuilder.DropForeignKey(
            name: "FK_Rooms_Homes_HomeId",
            table: "Rooms");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Rooms",
            table: "Rooms");

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"), new Guid("17133752-d60e-4f35-916f-6651ab4463e4") });

        migrationBuilder.DropColumn(
            name: "CreationDate",
            table: "Users");

        migrationBuilder.RenameTable(
            name: "Rooms",
            newName: "Room");

        migrationBuilder.RenameIndex(
            name: "IX_Rooms_HomeId",
            table: "Room",
            newName: "IX_Room_HomeId");

        migrationBuilder.AlterColumn<string>(
            name: "Type",
            table: "Devices",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_Room",
            table: "Room",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_HomeDevices_Room_RoomId",
            table: "HomeDevices",
            column: "RoomId",
            principalTable: "Room",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Room_Homes_HomeId",
            table: "Room",
            column: "HomeId",
            principalTable: "Homes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
