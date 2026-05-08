using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maia.Migrations
{
    /// <inheritdoc />
    public partial class AddKidsCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "KidsViewAllCards");

            migrationBuilder.AddColumn<int>(
                name: "KidsCategoryId",
                table: "KidsViewAllCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_KidsViewAllCards_KidsCategoryId",
                table: "KidsViewAllCards",
                column: "KidsCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_KidsViewAllCards_KidsCategories_KidsCategoryId",
                table: "KidsViewAllCards",
                column: "KidsCategoryId",
                principalTable: "KidsCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KidsViewAllCards_KidsCategories_KidsCategoryId",
                table: "KidsViewAllCards");

            migrationBuilder.DropTable(
                name: "KidsCategories");

            migrationBuilder.DropIndex(
                name: "IX_KidsViewAllCards_KidsCategoryId",
                table: "KidsViewAllCards");

            migrationBuilder.DropColumn(
                name: "KidsCategoryId",
                table: "KidsViewAllCards");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "KidsViewAllCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
