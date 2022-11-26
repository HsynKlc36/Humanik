using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanIK.REPOSITORIES.Migrations
{
    public partial class advance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfEmployment",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Advances",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfRequest = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CurrencyUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfReply = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmationStatus = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DemandOwnerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advances", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Advances_AspNetUsers_DemandOwnerId",
                        column: x => x.DemandOwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advances_DemandOwnerId",
                table: "Advances",
                column: "DemandOwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advances");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfEmployment",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }
    }
}
