using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanIK.REPOSITORIES.Migrations
{
    public partial class IsEverLoggedInEklendi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEverLoggedIn",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEverLoggedIn",
                table: "AspNetUsers");
        }
    }
}
