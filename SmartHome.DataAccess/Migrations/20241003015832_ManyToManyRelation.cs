using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class ManyToManyRelation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_HomePermissions_HomeMembers_HomeMemberId",
            table: "HomePermissions");

        migrationBuilder.DropForeignKey(
            name: "FK_SystemPermissions_Roles_RoleId",
            table: "SystemPermissions");

        migrationBuilder.DropIndex(
            name: "IX_SystemPermissions_RoleId",
            table: "SystemPermissions");

        migrationBuilder.DropIndex(
            name: "IX_HomePermissions_HomeMemberId",
            table: "HomePermissions");

        migrationBuilder.DropColumn(
            name: "RoleId",
            table: "SystemPermissions");

        migrationBuilder.DropColumn(
            name: "HomeMemberId",
            table: "HomePermissions");

        migrationBuilder.CreateTable(
            name: "HomeMemberPermission",
            columns: table => new
            {
                HomeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomeMemberPermission", x => new { x.HomeMemberId, x.PermissionId });
                table.ForeignKey(
                    name: "FK_HomeMemberPermission_HomeMembers_HomeMemberId",
                    column: x => x.HomeMemberId,
                    principalTable: "HomeMembers",
                    principalColumn: "HomeMemberId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_HomeMemberPermission_HomePermissions_PermissionId",
                    column: x => x.PermissionId,
                    principalTable: "HomePermissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RoleSystemPermission",
            columns: table => new
            {
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SystemPermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleSystemPermission", x => new { x.RoleId, x.SystemPermissionId });
                table.ForeignKey(
                    name: "FK_RoleSystemPermission_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RoleSystemPermission_SystemPermissions_SystemPermissionId",
                    column: x => x.SystemPermissionId,
                    principalTable: "SystemPermissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_HomeMemberPermission_PermissionId",
            table: "HomeMemberPermission",
            column: "PermissionId");

        migrationBuilder.CreateIndex(
            name: "IX_RoleSystemPermission_SystemPermissionId",
            table: "RoleSystemPermission",
            column: "SystemPermissionId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "HomeMemberPermission");

        migrationBuilder.DropTable(
            name: "RoleSystemPermission");

        migrationBuilder.AddColumn<Guid>(
            name: "RoleId",
            table: "SystemPermissions",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "HomeMemberId",
            table: "HomePermissions",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_SystemPermissions_RoleId",
            table: "SystemPermissions",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_HomePermissions_HomeMemberId",
            table: "HomePermissions",
            column: "HomeMemberId");

        migrationBuilder.AddForeignKey(
            name: "FK_HomePermissions_HomeMembers_HomeMemberId",
            table: "HomePermissions",
            column: "HomeMemberId",
            principalTable: "HomeMembers",
            principalColumn: "HomeMemberId");

        migrationBuilder.AddForeignKey(
            name: "FK_SystemPermissions_Roles_RoleId",
            table: "SystemPermissions",
            column: "RoleId",
            principalTable: "Roles",
            principalColumn: "Id");
    }
}
