using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MenSection.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MenCategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenCards_MenCategories_MenCategoryId",
                        column: x => x.MenCategoryId,
                        principalTable: "MenCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MenCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "T-Shirts" },
                    { 2, "Jackets" },
                    { 3, "Jeans" },
                    { 4, "Shirts" }
                });

            migrationBuilder.InsertData(
                table: "MenCards",
                columns: new[] { "Id", "Description", "ImageUrl", "MenCategoryId", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "Essential white t-shirt", "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400&q=80", 1, 20m, "Classic White Tee" },
                    { 2, "Streetwear graphic t-shirt", "https://images.unsplash.com/photo-1503341504253-dff4815485f1?w=400&q=80", 1, 25m, "Black Graphic Tee" },
                    { 3, "Classic leather jacket", "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=400&q=80", 2, 150m, "Leather Jacket" },
                    { 4, "Casual denim jacket", "https://images.unsplash.com/photo-1523205771623-e0faa4d2813d?w=400&q=80", 2, 89m, "Denim Jacket" },
                    { 5, "Modern slim fit jeans", "https://images.unsplash.com/photo-1542272454315-4c01d7abdf4a?w=400&q=80", 3, 55m, "Slim Fit Jeans" },
                    { 6, "Sleek black skinny jeans", "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=400&q=80", 3, 60m, "Black Skinny Jeans" },
                    { 7, "Smart Oxford shirt", "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=400&q=80", 4, 45m, "Oxford Shirt" },
                    { 8, "Casual flannel shirt", "https://images.unsplash.com/photo-1589310243389-96a5483213a8?w=400&q=80", 4, 38m, "Flannel Shirt" },
                    { 9, "Stylish bomber jacket", "https://images.unsplash.com/photo-1520975916090-3105956dac38?w=400&q=80", 2, 110m, "Bomber Jacket" },
                    { 10, "Light summer linen shirt", "https://images.unsplash.com/photo-1602810318383-e386cc2a3ccf?w=400&q=80", 4, 42m, "Linen Shirt" },
                    { 11, "Classic white polo shirt", "https://images.unsplash.com/photo-1618354691373-d851c5c827a4?w=400&q=80", 1, 30m, "White Polo Shirt" },
                    { 12, "Comfortable casual chinos", "https://images.unsplash.com/photo-1473966968600-fa4cebea7cf0?w=400&q=80", 3, 65m, "Casual Chinos" },
                    { 13, "Warm puffer jacket", "https://images.unsplash.com/photo-1544966503-7b654b25498d?w=400&q=80", 2, 130m, "Puffer Jacket" },
                    { 14, "Casual striped shirt", "https://images.unsplash.com/photo-1552374196-1ab2a1c593e8?w=400&q=80", 4, 48m, "Striped Shirt" },
                    { 15, "Modern dark wash jeans", "https://images.unsplash.com/photo-1598033129183-c4f50c736f10?w=400&q=80", 3, 70m, "Dark Wash Jeans" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenCards_MenCategoryId",
                table: "MenCards",
                column: "MenCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenCards");

            migrationBuilder.DropTable(
                name: "MenCategories");
        }
    }
}
