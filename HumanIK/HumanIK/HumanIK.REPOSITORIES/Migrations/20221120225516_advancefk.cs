using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanIK.REPOSITORIES.Migrations
{
    public partial class advancefk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advances_AspNetUsers_DemandOwnerId",
                table: "Advances");

            migrationBuilder.AlterColumn<int>(
                name: "DemandOwnerId",
                table: "Advances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Advances_AspNetUsers_DemandOwnerId",
                table: "Advances",
                column: "DemandOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advances_AspNetUsers_DemandOwnerId",
                table: "Advances");

            migrationBuilder.AlterColumn<int>(
                name: "DemandOwnerId",
                table: "Advances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Advances_AspNetUsers_DemandOwnerId",
                table: "Advances",
                column: "DemandOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
