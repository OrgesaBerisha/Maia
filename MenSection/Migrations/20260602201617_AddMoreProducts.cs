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
                    { 1, "Essential white t-shirt for everyday wear", "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400&q=80", 1, 20m, "Classic White Tee" },
                    { 2, "Bold streetwear graphic t-shirt", "https://images.unsplash.com/photo-1503341504253-dff4815485f1?w=400&q=80", 1, 25m, "Black Graphic Tee" },
                    { 3, "Classic navy blue crew neck t-shirt", "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?w=400&q=80", 1, 22m, "Navy Blue Tee" },
                    { 4, "Smart striped polo for casual outings", "https://images.unsplash.com/photo-1581655353564-df123a1eb820?w=400&q=80", 1, 35m, "Striped Polo Shirt" },
                    { 5, "Classic black leather jacket with zip closure", "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=400&q=80", 2, 150m, "Leather Jacket" },
                    { 6, "Timeless casual denim jacket", "https://images.unsplash.com/photo-1523205771623-e0faa4d2813d?w=400&q=80", 2, 89m, "Denim Jacket" },
                    { 7, "Stylish bomber jacket perfect for autumn", "https://images.unsplash.com/photo-1520975916090-3105956dac38?w=400&q=80", 2, 110m, "Bomber Jacket" },
                    { 8, "Warm puffer jacket for cold winter days", "https://images.unsplash.com/photo-1547624643-3bf761b09502?w=400&q=80", 2, 130m, "Puffer Jacket" },
                    { 9, "Modern slim fit blue jeans", "https://images.unsplash.com/photo-1542272454315-4c01d7abdf4a?w=400&q=80", 3, 55m, "Slim Fit Jeans" },
                    { 10, "Sleek black skinny jeans for a sharp look", "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=400&q=80", 3, 60m, "Black Skinny Jeans" },
                    { 11, "Comfortable relaxed fit jeans for casual days", "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?w=400&q=80", 3, 50m, "Relaxed Fit Jeans" },
                    { 12, "Trendy ripped jeans with distressed details", "https://images.unsplash.com/photo-1555689502-c4b22d76c56f?w=400&q=80", 3, 65m, "Ripped Jeans" },
                    { 13, "Smart Oxford shirt for office or casual wear", "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=400&q=80", 4, 45m, "Oxford Shirt" },
                    { 14, "Cozy flannel shirt perfect for weekends", "https://images.unsplash.com/photo-1589310243389-96a5483213a8?w=400&q=80", 4, 38m, "Flannel Shirt" },
                    { 15, "Light breathable linen shirt for summer", "https://images.unsplash.com/photo-1602810318383-e386cc2a3ccf?w=400&q=80", 4, 42m, "Linen Shirt" },
                    { 16, "Elegant black formal shirt for special occasions", "https://images.unsplash.com/photo-1598033129183-c4f50c736f10?w=400&q=80", 4, 48m, "Black Formal Shirt" }
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
