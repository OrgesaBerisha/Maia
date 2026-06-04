using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maia.Migrations
{
    /// <inheritdoc />
    public partial class FixKidsCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Table was already created as CardsWomen in a prior migration — no-op.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardsWomen_WomanCategories_WomanCategoryId",
                table: "CardsWomen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardsWomen",
                table: "CardsWomen");

            migrationBuilder.RenameTable(
                name: "CardsWomen",
                newName: "CardsWoman");

            migrationBuilder.RenameIndex(
                name: "IX_CardsWomen_WomanCategoryId",
                table: "CardsWoman",
                newName: "IX_CardsWoman_WomanCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardsWoman",
                table: "CardsWoman",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardsWoman_WomanCategories_WomanCategoryId",
                table: "CardsWoman",
                column: "WomanCategoryId",
                principalTable: "WomanCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
