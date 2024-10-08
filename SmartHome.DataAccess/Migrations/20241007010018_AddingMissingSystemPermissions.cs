using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddingMissingSystemPermissions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "Time",
            table: "Notifications",
            type: "datetime2",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddColumn<Guid>(
            name: "DetectedPersonId",
            table: "Notifications",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.InsertData(
            table: "SystemPermissions",
            columns: new[] { "Id", "Description", "Name" },
            values: new object[,]
            {
                { new Guid("0957ea49-6c58-4f9a-9cbc-bb4a300077f3"), "List all users homes", "List all users homes" },
                { new Guid("48644f45-4d83-4961-924b-7733881258e9"), "Lets a user with the role homeOwner access the home related endpoints", "Home related permission" },
                { new Guid("b404659a-35a4-4486-867f-db4c24f9f827"), "Create notification", "Create notification" },
                { new Guid("dde0bac9-e646-4d9f-96f5-c77e7295cb4b"), "Lets a user access all its notifications", "List all users notifications" }
            });

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleId", "SystemPermissionId" },
            values: new object[,]
            {
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("0957ea49-6c58-4f9a-9cbc-bb4a300077f3") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("48644f45-4d83-4961-924b-7733881258e9") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("b404659a-35a4-4486-867f-db4c24f9f827") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("dde0bac9-e646-4d9f-96f5-c77e7295cb4b") }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_DetectedPersonId",
            table: "Notifications",
            column: "DetectedPersonId");

        migrationBuilder.AddForeignKey(
            name: "FK_Notifications_Users_DetectedPersonId",
            table: "Notifications",
            column: "DetectedPersonId",
            principalTable: "Users",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Notifications_Users_DetectedPersonId",
            table: "Notifications");

        migrationBuilder.DropIndex(
            name: "IX_Notifications_DetectedPersonId",
            table: "Notifications");

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("0957ea49-6c58-4f9a-9cbc-bb4a300077f3") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("48644f45-4d83-4961-924b-7733881258e9") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("b404659a-35a4-4486-867f-db4c24f9f827") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("dde0bac9-e646-4d9f-96f5-c77e7295cb4b") });

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("0957ea49-6c58-4f9a-9cbc-bb4a300077f3"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("48644f45-4d83-4961-924b-7733881258e9"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("b404659a-35a4-4486-867f-db4c24f9f827"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("dde0bac9-e646-4d9f-96f5-c77e7295cb4b"));

        migrationBuilder.DropColumn(
            name: "DetectedPersonId",
            table: "Notifications");

        migrationBuilder.AlterColumn<string>(
            name: "Time",
            table: "Notifications",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "datetime2");
    }
}
