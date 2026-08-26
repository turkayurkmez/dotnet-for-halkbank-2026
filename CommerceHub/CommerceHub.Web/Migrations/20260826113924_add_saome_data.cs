using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CommerceHub.Web.Migrations
{
    /// <inheritdoc />
    public partial class add_saome_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Bilgisayar" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BasePrice", "CategoryId", "Description", "DiscountRate", "IsOnSale", "Name", "StockCount" },
                values: new object[,]
                {
                    { 1, 2000m, 1, "Logitech Bluetooth", 0.25, true, "Kablosuz Klavye", 100 },
                    { 2, 250m, 1, "Gamer mouse", 0.0, false, "Kablolu Mouse", 100 },
                    { 3, 6000m, 1, "MSI", 0.14999999999999999, true, "24'' monitör", 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
