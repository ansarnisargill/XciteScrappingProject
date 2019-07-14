using Microsoft.EntityFrameworkCore.Migrations;

namespace DataScrapingApp.Migrations
{
    public partial class addDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "data",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuickOverview",
                table: "data",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "data");

            migrationBuilder.DropColumn(
                name: "QuickOverview",
                table: "data");
        }
    }
}
