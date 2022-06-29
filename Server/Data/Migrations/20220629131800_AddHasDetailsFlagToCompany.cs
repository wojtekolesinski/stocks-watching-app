using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stocks.Server.Data.Migrations
{
    public partial class AddHasDetailsFlagToCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCompany",
                table: "Articles");

            migrationBuilder.AddColumn<bool>(
                name: "HasDetails",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasDetails",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "IdCompany",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
