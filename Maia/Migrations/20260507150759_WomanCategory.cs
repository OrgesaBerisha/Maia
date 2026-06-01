using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maia.Migrations
{
    /// <inheritdoc />
    public partial class WomanCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CardsWoman",
                table: "CardsWoman");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "CardsWoman");

            migrationBuilder.RenameTable(
                name: "CardsWoman",
                newName: "CardsWomen");

            migrationBuilder.AddColumn<int>(
                name: "WomanCategoryId",
                table: "CardsWomen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardsWomen",
                table: "CardsWomen",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WomanCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WomanCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardsWomen_WomanCategoryId",
                table: "CardsWomen",
                column: "WomanCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardsWomen_WomanCategories_WomanCategoryId",
                table: "CardsWomen",
                column: "WomanCategoryId",
                principalTable: "WomanCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardsWomen_WomanCategories_WomanCategoryId",
                table: "CardsWomen");

            migrationBuilder.DropTable(
                name: "WomanCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardsWomen",
                table: "CardsWomen");

            migrationBuilder.DropIndex(
                name: "IX_CardsWomen_WomanCategoryId",
                table: "CardsWomen");

            migrationBuilder.DropColumn(
                name: "WomanCategoryId",
                table: "CardsWomen");

            migrationBuilder.RenameTable(
                name: "CardsWomen",
                newName: "CardsWoman");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "CardsWoman",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardsWoman",
                table: "CardsWoman",
                column: "Id");
        }
    }
}
