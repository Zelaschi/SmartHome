using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.DataAccess.Migrations;

/// <inheritdoc />
public partial class HomeMemberNotificationsRestructure : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Devices_Businesses_BusinessId",
            table: "Devices");

        migrationBuilder.AlterColumn<Guid>(
            name: "BusinessId",
            table: "Devices",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddForeignKey(
            name: "FK_Devices_Businesses_BusinessId",
            table: "Devices",
            column: "BusinessId",
            principalTable: "Businesses",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Devices_Businesses_BusinessId",
            table: "Devices");

        migrationBuilder.AlterColumn<Guid>(
            name: "BusinessId",
            table: "Devices",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Devices_Businesses_BusinessId",
            table: "Devices",
            column: "BusinessId",
            principalTable: "Businesses",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
