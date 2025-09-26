using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrenzoStore.Migrations
{
    /// <inheritdoc />
    public partial class AddProductImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "AltText", "ImageUrl", "IsPrimary", "ProductId", "SortOrder" },
                values: new object[,]
                {
                    { 1, "Classic White T-Shirt - Front View", "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=500&h=500&fit=crop", true, 1, 1 },
                    { 2, "Classic White T-Shirt - Side View", "https://images.unsplash.com/photo-1583743814966-8936f37f4678?w=500&h=500&fit=crop", false, 1, 2 },
                    { 3, "Blue Denim Jeans - Front View", "https://images.unsplash.com/photo-1542272604-787c3835535d?w=500&h=500&fit=crop", true, 2, 1 },
                    { 4, "Blue Denim Jeans - Detail View", "https://images.unsplash.com/photo-1475178626620-a4d074967452?w=500&h=500&fit=crop", false, 2, 2 },
                    { 5, "Elegant Black Dress - Front View", "https://images.unsplash.com/photo-1566479179817-c0ae2d4d6b3e?w=500&h=500&fit=crop", true, 3, 1 },
                    { 6, "Elegant Black Dress - Full Length", "https://images.unsplash.com/photo-1515372039744-b8f02a3ae446?w=500&h=500&fit=crop", false, 3, 2 },
                    { 7, "Leather Handbag - Main View", "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&h=500&fit=crop", true, 4, 1 },
                    { 8, "Leather Handbag - Detail View", "https://images.unsplash.com/photo-1584917865442-de89df76afd3?w=500&h=500&fit=crop", false, 4, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
