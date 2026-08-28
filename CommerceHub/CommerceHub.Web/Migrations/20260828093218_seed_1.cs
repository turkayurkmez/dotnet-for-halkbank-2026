using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommerceHub.Web.Migrations
{
    /// <inheritdoc />
    public partial class seed_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "SKU",
                value: "logi-keyb-1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "SKU",
                value: null);
        }
    }
}
