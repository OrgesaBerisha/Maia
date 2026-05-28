using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WomensSection.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WomanCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WomanCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardsWomen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    WomanCategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardsWomen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardsWomen_WomanCategories_WomanCategoryId",
                        column: x => x.WomanCategoryId,
                        principalTable: "WomanCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "WomanCategories",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 27, 21, 21, 9, 713, DateTimeKind.Utc).AddTicks(1881), "Dresses", null },
                    { 2, new DateTime(2026, 5, 27, 21, 21, 9, 713, DateTimeKind.Utc).AddTicks(2391), "Shoes", null },
                    { 3, new DateTime(2026, 5, 27, 21, 21, 9, 713, DateTimeKind.Utc).AddTicks(2392), "Jackets", null },
                    { 4, new DateTime(2026, 5, 27, 21, 21, 9, 713, DateTimeKind.Utc).AddTicks(2393), "Bags", null }
                });

            migrationBuilder.InsertData(
                table: "CardsWomen",
                columns: new[] { "Id", "CreatedAt", "Description", "ImageUrl", "Price", "Title", "UpdatedAt", "WomanCategoryId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Light summer dress", "...", 25m, "Summer Dress", null, 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sport shoes", "...", 80m, "Nike Shoes", null, 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warm jacket", "...", 120m, "Winter Jacket", null, 3 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fashion bag", "...", 200m, "Luxury Bag", null, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardsWomen_WomanCategoryId",
                table: "CardsWomen",
                column: "WomanCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardsWomen");

            migrationBuilder.DropTable(
                name: "WomanCategories");
        }
    }
}
