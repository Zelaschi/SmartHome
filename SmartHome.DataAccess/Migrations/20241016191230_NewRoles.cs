using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), "BusinessOwnerHomeOwner" },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), "AdminHomeOwner" }
                });

            migrationBuilder.InsertData(
                table: "RoleSystemPermission",
                columns: new[] { "RoleId", "SystemPermissionId" },
                values: new object[,]
                {
                    { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("0957ea49-6c58-4f9a-9cbc-bb4a300077f3") },
                    { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("48644f45-4d83-4961-924b-7733881258e9") },
                    { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("7c1d3527-e47c-43ac-b979-447a05558f25") },
                    { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("b404659a-35a4-4486-867f-db4c24f9f827") },
                    { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("dde0bac9-e646-4d9f-96f5-c77e7295cb4b") },
                    { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("17133752-d60e-4f35-916f-6651ab4463e4") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("4e2851c0-c5bb-4e52-b6a5-badadbbd83dd") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("b3eef741-8d56-4263-a633-7e176981feec") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("b6f31ea3-aca9-4757-9905-eff4ef100564") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73") },
                    { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("f22942d7-9bc0-4458-a713-15c9010deaa1") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("0957ea49-6c58-4f9a-9cbc-bb4a300077f3") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("48644f45-4d83-4961-924b-7733881258e9") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("7c1d3527-e47c-43ac-b979-447a05558f25") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("b404659a-35a4-4486-867f-db4c24f9f827") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("dde0bac9-e646-4d9f-96f5-c77e7295cb4b") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"), new Guid("f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("17133752-d60e-4f35-916f-6651ab4463e4") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("4e2851c0-c5bb-4e52-b6a5-badadbbd83dd") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("b3eef741-8d56-4263-a633-7e176981feec") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("b6f31ea3-aca9-4757-9905-eff4ef100564") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73") });

            migrationBuilder.DeleteData(
                table: "RoleSystemPermission",
                keyColumns: new[] { "RoleId", "SystemPermissionId" },
                keyValues: new object[] { new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"), new Guid("f22942d7-9bc0-4458-a713-15c9010deaa1") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3b3b3b3-3c8a-423d-bd46-edde4f85fbe3"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f3b3b3b3-3c8a-423d-bd46-edde4f85fbe4"));
        }
    }
}
