using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrenzoStore.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreProductsAndFixImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1539008835657-9e8e9680c956?w=500&h=500&fit=crop");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1594633312681-425c7b97ccd1?w=500&h=500&fit=crop");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CompareAtPrice", "DateCreated", "DateModified", "Description", "IsActive", "IsFeatured", "Name", "Price", "SKU", "ShortDescription", "StockQuantity" },
                values: new object[,]
                {
                    { 5, 4, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comfortable white sneakers perfect for everyday wear. Made with breathable materials and cushioned sole.", true, true, "Casual Sneakers", 69.99m, "SNK-WHT-001", "Comfortable white sneakers", 45 },
                    { 6, 2, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beautiful floral print dress perfect for summer occasions. Light and airy fabric with a flattering fit.", true, true, "Summer Floral Dress", 95.99m, "DRS-FLR-001", "Beautiful floral summer dress", 20 },
                    { 7, 1, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Classic denim jacket with a modern fit. Perfect for layering and casual styling.", true, false, "Denim Jacket", 85.99m, "JKT-DNM-001", "Classic denim jacket", 35 },
                    { 8, 3, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luxurious silk scarf with elegant patterns. Perfect accessory for any outfit.", true, false, "Silk Scarf", 45.99m, "SCF-SLK-001", "Luxurious silk scarf", 25 },
                    { 9, 4, 179.99m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium leather boots with excellent craftsmanship. Durable and stylish for any season.", true, true, "Leather Boots", 149.99m, "BOT-LTH-001", "Premium leather boots", 15 },
                    { 10, 1, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Classic cotton polo shirt in navy blue. Comfortable fit and timeless style.", true, false, "Cotton Polo Shirt", 39.99m, "PLO-NVY-001", "Classic cotton polo shirt", 60 }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "AltText", "ImageUrl", "IsPrimary", "ProductId", "SortOrder" },
                values: new object[,]
                {
                    { 9, "Casual White Sneakers - Side View", "https://images.unsplash.com/photo-1549298916-b41d501d3772?w=500&h=500&fit=crop", true, 5, 1 },
                    { 10, "Casual White Sneakers - Front View", "https://images.unsplash.com/photo-1595950653106-6c9ebd614d3a?w=500&h=500&fit=crop", false, 5, 2 },
                    { 11, "Summer Floral Dress - Front View", "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=500&h=500&fit=crop", true, 6, 1 },
                    { 12, "Summer Floral Dress - Detail View", "https://images.unsplash.com/photo-1583496661160-fb5886a13d24?w=500&h=500&fit=crop", false, 6, 2 },
                    { 13, "Denim Jacket - Front View", "https://images.unsplash.com/photo-1551698618-1dfe5d97d256?w=500&h=500&fit=crop", true, 7, 1 },
                    { 14, "Denim Jacket - Back View", "https://images.unsplash.com/photo-1576871337622-98d48d1cf531?w=500&h=500&fit=crop", false, 7, 2 },
                    { 15, "Silk Scarf - Pattern View", "https://images.unsplash.com/photo-1601924994987-69e26d50dc26?w=500&h=500&fit=crop", true, 8, 1 },
                    { 16, "Silk Scarf - Styled View", "https://images.unsplash.com/photo-1590736969955-71cc94901144?w=500&h=500&fit=crop", false, 8, 2 },
                    { 17, "Leather Boots - Side View", "https://images.unsplash.com/photo-1608256246200-53e635b5b65f?w=500&h=500&fit=crop", true, 9, 1 },
                    { 18, "Leather Boots - Detail View", "https://images.unsplash.com/photo-1544966503-7cc5ac882d5f?w=500&h=500&fit=crop", false, 9, 2 },
                    { 19, "Cotton Polo Shirt - Front View", "https://images.unsplash.com/photo-1586790170083-2f9ceadc732d?w=500&h=500&fit=crop", true, 10, 1 },
                    { 20, "Cotton Polo Shirt - Detail View", "https://images.unsplash.com/photo-1618354691373-d851c5c3a990?w=500&h=500&fit=crop", false, 10, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1566479179817-c0ae2d4d6b3e?w=500&h=500&fit=crop");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://images.unsplash.com/photo-1515372039744-b8f02a3ae446?w=500&h=500&fit=crop");
        }
    }
}
