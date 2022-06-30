using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stocks.Server.Data.Migrations
{
    public partial class AddPremarketAndAfterHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AfterHours",
                table: "StockPrices",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasDailyData",
                table: "StockPrices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PreMarket",
                table: "StockPrices",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfterHours",
                table: "StockPrices");

            migrationBuilder.DropColumn(
                name: "HasDailyData",
                table: "StockPrices");

            migrationBuilder.DropColumn(
                name: "PreMarket",
                table: "StockPrices");
        }
    }
}
