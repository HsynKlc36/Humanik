using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanIK.REPOSITORIES.Migrations
{
    public partial class _ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AspNetUsers_DemandOwnerId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "DemandOwnerId",
                table: "Expenses",
                newName: "ExpenseDemandOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_DemandOwnerId",
                table: "Expenses",
                newName: "IX_Expenses_ExpenseDemandOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AspNetUsers_ExpenseDemandOwnerId",
                table: "Expenses",
                column: "ExpenseDemandOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AspNetUsers_ExpenseDemandOwnerId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "ExpenseDemandOwnerId",
                table: "Expenses",
                newName: "DemandOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_ExpenseDemandOwnerId",
                table: "Expenses",
                newName: "IX_Expenses_DemandOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AspNetUsers_DemandOwnerId",
                table: "Expenses",
                column: "DemandOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
