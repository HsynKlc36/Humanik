using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanIK.REPOSITORIES.Migrations
{
    public partial class currencyUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalaryCurrencyUnit",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyUnit",
                table: "Advances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryCurrencyUnit",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyUnit",
                table: "Advances",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
