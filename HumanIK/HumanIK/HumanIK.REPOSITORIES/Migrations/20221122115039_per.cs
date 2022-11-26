using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanIK.REPOSITORIES.Migrations
{
    public partial class per : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfPermission",
                table: "Permission",
                newName: "NumberOfDays");

            migrationBuilder.AddColumn<int>(
                name: "ConfirmationStatus",
                table: "Permission",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfReply",
                table: "Permission",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationStatus",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "DateOfReply",
                table: "Permission");

            migrationBuilder.RenameColumn(
                name: "NumberOfDays",
                table: "Permission",
                newName: "NumberOfPermission");
        }
    }
}
