using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidsSection.Migrations
{
    /// <inheritdoc />
    public partial class KidsCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KidsCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KidsCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KidsCategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidsCards_KidsCategories_KidsCategoryId",
                        column: x => x.KidsCategoryId,
                        principalTable: "KidsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KidsCards_KidsCategoryId",
                table: "KidsCards",
                column: "KidsCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KidsCards");

            migrationBuilder.DropTable(
                name: "KidsCategories");
        }
    }
}
