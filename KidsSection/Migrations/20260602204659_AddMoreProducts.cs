using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KidsSection.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "KidsCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "T-Shirts" },
                    { 2, "Dresses" },
                    { 3, "Jeans" },
                    { 4, "Shorts" }
                });

            migrationBuilder.InsertData(
                table: "KidsProductTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Boys" },
                    { 2, "Girls" }
                });

            migrationBuilder.InsertData(
                table: "KidsCards",
                columns: new[] { "Id", "Description", "ImageUrl", "KidsCategoryId", "KidsProductTypeId", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "Colorful striped t-shirt", "https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80", 1, 1, 12m, "Striped T-Shirt" },
                    { 2, "Pretty floral dress", "https://images.unsplash.com/photo-1508766917616-d22f3f1eea14?w=400&q=80", 2, 2, 18m, "Floral Dress" },
                    { 3, "Classic denim jeans", "https://images.unsplash.com/photo-1503454537195-1dcabb73ffb9?w=400&q=80", 3, 1, 22m, "Denim Jeans" },
                    { 4, "Light summer shorts", "https://images.unsplash.com/photo-1471286174890-9c112ac6f609?w=400&q=80", 4, 2, 14m, "Summer Shorts" },
                    { 5, "Fun rainbow hoodie", "https://images.unsplash.com/photo-1556821840-3a63f15732ce?w=400&q=80", 1, 1, 20m, "Rainbow Hoodie" },
                    { 6, "Lovely princess dress", "https://images.unsplash.com/photo-1518831959646-742c3a14ebf7?w=400&q=80", 2, 2, 25m, "Princess Dress" },
                    { 7, "Practical cargo shorts", "https://images.unsplash.com/photo-1591195853828-11db59a44f43?w=400&q=80", 4, 1, 16m, "Cargo Shorts" },
                    { 8, "Comfortable leggings", "https://images.unsplash.com/photo-1543087903-1ac2ec7aa8c5?w=400&q=80", 3, 2, 15m, "Leggings" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "KidsProductTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KidsProductTypes",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
