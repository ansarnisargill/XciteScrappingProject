using Microsoft.EntityFrameworkCore.Migrations;

namespace DataScrapingApp.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisited",
                table: "data",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "arabicData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(nullable: true),
                    PrentCategories = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    BrandName = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    ImageLink = table.Column<string>(nullable: true),
                    QuickOverview = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arabicData", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "arabicData");

            migrationBuilder.DropColumn(
                name: "IsVisited",
                table: "data");
        }
    }
}
