using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "HomePermissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), "Create room permission" },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), "Add device to room permission" }
                });

            migrationBuilder.InsertData(
                table: "RoleSystemPermission",
                columns: new[] { "RoleId", "SystemPermissionId" },
                values: new object[,]
                {
                    { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("17133752-d60e-4f35-916f-6651ab4463e4") },
                    { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360") },
                    { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b") },
                    { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("0957ea49-6c58-4f9a-9cbc-bb4a300077f3") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("48644f45-4d83-4961-924b-7733881258e9") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("b404659a-35a4-4486-867f-db4c24f9f827") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("dde0bac9-e646-4d9f-96f5-c77e7295cb4b") }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("80e909fb-3c8a-423d-bd46-edde4f85fbe3"),
                column: "CreationDate",
                value: new DateTime(2024, 11, 16, 1, 20, 41, 970, DateTimeKind.Local).AddTicks(9435));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HomePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"));

            migrationBuilder.DeleteData(
                table: "HomePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"));

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("17133752-d60e-4f35-916f-6651ab4463e4") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("0957ea49-6c58-4f9a-9cbc-bb4a300077f3") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("48644f45-4d83-4961-924b-7733881258e9") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("b404659a-35a4-4486-867f-db4c24f9f827") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("dde0bac9-e646-4d9f-96f5-c77e7295cb4b") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("80e909fb-3c8a-423d-bd46-edde4f85fbe3"),
                column: "CreationDate",
                value: new DateTime(2024, 11, 12, 15, 40, 24, 339, DateTimeKind.Local).AddTicks(8586));
        }
    }
}
