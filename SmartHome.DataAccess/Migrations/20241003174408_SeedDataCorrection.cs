using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class SeedDataCorrection : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Roles_RoleId",
            table: "Users");

        migrationBuilder.AlterColumn<Guid>(
            name: "RoleId",
            table: "Users",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.InsertData(
            table: "HomePermissions",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                { new Guid("98bb8133-688a-4f1d-8587-87c485df6534"), "Add member to home permission" },
                { new Guid("9d7f6847-e8d5-4515-b9ac-0f0c00fcc7b3"), "Recieve device's notifications" },
                { new Guid("c49f2858-72fc-422d-bd4b-f49b482f80bd"), "Add devices to home permission" },
                { new Guid("fa0cad23-153b-46b5-a690-91d0d7677c31"), "List home's devices permission" }
            });

        migrationBuilder.InsertData(
            table: "Roles",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                { new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"), "BusinessOwner" },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), "HomeOwner" },
                { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), "Admin" }
            });

        migrationBuilder.InsertData(
            table: "SystemPermissions",
            columns: new[] { "Id", "Description", "Name" },
            values: new object[,]
            {
                { new Guid("17133752-d60e-4f35-916f-6651ab4463e4"), "List all device types", "List all device types" },
                { new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360"), "Create home", "Create home" },
                { new Guid("4e2851c0-c5bb-4e52-b6a5-badadbbd83dd"), "List all businesses", "List all businesses" },
                { new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b"), "Add member to home", "Add member to home" },
                { new Guid("7c1d3527-e47c-43ac-b979-447a05558f25"), "Create device", "Create device" },
                { new Guid("b3eef741-8d56-4263-a633-7e176981feec"), "Create business owner account", "Create business owner account" },
                { new Guid("b6f31ea3-aca9-4757-9905-eff4ef100564"), "List all accounts", "List all accounts" },
                { new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73"), "List all devices", "List all devices" },
                { new Guid("f22942d7-9bc0-4458-a713-15c9010deaa1"), "Create or delete admin account", "Create or delete admin account" },
                { new Guid("f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95"), "Create business", "Create business" }
            });

        migrationBuilder.InsertData(
            table: "RoleSystemPermission",
            columns: new[] { "RoleId", "SystemPermissionId" },
            values: new object[,]
            {
                { new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"), new Guid("7c1d3527-e47c-43ac-b979-447a05558f25") },
                { new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"), new Guid("f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("17133752-d60e-4f35-916f-6651ab4463e4") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b") },
                { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73") },
                { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("4e2851c0-c5bb-4e52-b6a5-badadbbd83dd") },
                { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("b3eef741-8d56-4263-a633-7e176981feec") },
                { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("b6f31ea3-aca9-4757-9905-eff4ef100564") },
                { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("f22942d7-9bc0-4458-a713-15c9010deaa1") }
            });

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "Complete", "Email", "Name", "Password", "ProfilePhoto", "RoleId", "Surname" },
            values: new object[] { new Guid("80e909fb-3c8a-423d-bd46-edde4f85fbe3"), null, "admin1234@gmail.com", "First admin", "Password@1234", null, new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), "admin surname" });

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Roles_RoleId",
            table: "Users",
            column: "RoleId",
            principalTable: "Roles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Roles_RoleId",
            table: "Users");

        migrationBuilder.DeleteData(
            table: "HomePermissions",
            keyColumn: "Id",
            keyValue: new Guid("98bb8133-688a-4f1d-8587-87c485df6534"));

        migrationBuilder.DeleteData(
            table: "HomePermissions",
            keyColumn: "Id",
            keyValue: new Guid("9d7f6847-e8d5-4515-b9ac-0f0c00fcc7b3"));

        migrationBuilder.DeleteData(
            table: "HomePermissions",
            keyColumn: "Id",
            keyValue: new Guid("c49f2858-72fc-422d-bd4b-f49b482f80bd"));

        migrationBuilder.DeleteData(
            table: "HomePermissions",
            keyColumn: "Id",
            keyValue: new Guid("fa0cad23-153b-46b5-a690-91d0d7677c31"));

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"), new Guid("7c1d3527-e47c-43ac-b979-447a05558f25") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"), new Guid("f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("17133752-d60e-4f35-916f-6651ab4463e4") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"), new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("4e2851c0-c5bb-4e52-b6a5-badadbbd83dd") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("b3eef741-8d56-4263-a633-7e176981feec") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("b6f31ea3-aca9-4757-9905-eff4ef100564") });

        migrationBuilder.DeleteData(
            table: "RoleSystemPermission",
            keyColumns: new[] { "RoleId", "SystemPermissionId" },
            keyValues: new object[] { new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"), new Guid("f22942d7-9bc0-4458-a713-15c9010deaa1") });

        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("80e909fb-3c8a-423d-bd46-edde4f85fbe3"));

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: new Guid("28a660d2-c86a-49a8-bbeb-587a82415771"));

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: new Guid("5725feee-327f-4147-aad9-ea28b9ff3e7b"));

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: new Guid("ffa636e8-ce76-4b52-b03e-8b3989bfd008"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("17133752-d60e-4f35-916f-6651ab4463e4"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("206ad2b6-e911-4491-84e4-0a6082f5f360"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("4e2851c0-c5bb-4e52-b6a5-badadbbd83dd"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("7306a4ce-47fc-4ba8-8aac-60243701cd5b"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("7c1d3527-e47c-43ac-b979-447a05558f25"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("b3eef741-8d56-4263-a633-7e176981feec"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("b6f31ea3-aca9-4757-9905-eff4ef100564"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("bf35d7ed-f4b9-410e-a427-d139ce74cf73"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("f22942d7-9bc0-4458-a713-15c9010deaa1"));

        migrationBuilder.DeleteData(
            table: "SystemPermissions",
            keyColumn: "Id",
            keyValue: new Guid("f7a9dcac-f312-4ad1-b3aa-1caa2ad7df95"));

        migrationBuilder.AlterColumn<Guid>(
            name: "RoleId",
            table: "Users",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Roles_RoleId",
            table: "Users",
            column: "RoleId",
            principalTable: "Roles",
            principalColumn: "Id");
    }
}
