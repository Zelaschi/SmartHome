using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class HomeMemberToNotificationMtMRelation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Businesses_Users_BusinessOwnerId",
            table: "Businesses");

        migrationBuilder.DropForeignKey(
            name: "FK_Notifications_HomeMembers_HomeMemberId",
            table: "Notifications");

        migrationBuilder.DropIndex(
            name: "IX_Notifications_HomeMemberId",
            table: "Notifications");

        migrationBuilder.DropColumn(
            name: "HomeMemberId",
            table: "Notifications");

        migrationBuilder.DropColumn(
            name: "Read",
            table: "Notifications");

        migrationBuilder.AlterColumn<Guid>(
            name: "BusinessOwnerId",
            table: "Businesses",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.CreateTable(
            name: "HomeMemberNotification",
            columns: table => new
            {
                NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HomeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Read = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomeMemberNotification", x => new { x.HomeMemberId, x.NotificationId });
                table.ForeignKey(
                    name: "FK_HomeMemberNotification_HomeMembers_HomeMemberId",
                    column: x => x.HomeMemberId,
                    principalTable: "HomeMembers",
                    principalColumn: "HomeMemberId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_HomeMemberNotification_Notifications_NotificationId",
                    column: x => x.NotificationId,
                    principalTable: "Notifications",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_HomeMemberNotification_NotificationId",
            table: "HomeMemberNotification",
            column: "NotificationId");

        migrationBuilder.AddForeignKey(
            name: "FK_Businesses_Users_BusinessOwnerId",
            table: "Businesses",
            column: "BusinessOwnerId",
            principalTable: "Users",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Businesses_Users_BusinessOwnerId",
            table: "Businesses");

        migrationBuilder.DropTable(
            name: "HomeMemberNotification");

        migrationBuilder.AddColumn<Guid>(
            name: "HomeMemberId",
            table: "Notifications",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "Read",
            table: "Notifications",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AlterColumn<Guid>(
            name: "BusinessOwnerId",
            table: "Businesses",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_HomeMemberId",
            table: "Notifications",
            column: "HomeMemberId");

        migrationBuilder.AddForeignKey(
            name: "FK_Businesses_Users_BusinessOwnerId",
            table: "Businesses",
            column: "BusinessOwnerId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Notifications_HomeMembers_HomeMemberId",
            table: "Notifications",
            column: "HomeMemberId",
            principalTable: "HomeMembers",
            principalColumn: "HomeMemberId");
    }
}
