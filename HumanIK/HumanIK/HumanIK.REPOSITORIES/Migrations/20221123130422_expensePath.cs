using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanIK.REPOSITORIES.Migrations
{
    public partial class expensePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpenseFilePath",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseFilePath",
                table: "Expenses");
        }
    }
}
