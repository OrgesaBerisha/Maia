using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WomensSection.Migrations
{
    /// <inheritdoc />
    public partial class WishlistMultiSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop FK and index to CardsWomen if they exist (safe to run even if they don't)
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM sys.foreign_keys
                    WHERE name = 'FK_WishlistItems_CardsWomen_ProductId'
                )
                ALTER TABLE [WishlistItems] DROP CONSTRAINT [FK_WishlistItems_CardsWomen_ProductId];
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'IX_WishlistItems_ProductId'
                    AND object_id = OBJECT_ID(N'WishlistItems')
                )
                DROP INDEX [IX_WishlistItems_ProductId] ON [WishlistItems];
            ");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "WishlistItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "WOMAN");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "WishlistItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductImage",
                table: "WishlistItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductPrice",
                table: "WishlistItems",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Source",       table: "WishlistItems");
            migrationBuilder.DropColumn(name: "ProductName",  table: "WishlistItems");
            migrationBuilder.DropColumn(name: "ProductImage", table: "WishlistItems");
            migrationBuilder.DropColumn(name: "ProductPrice", table: "WishlistItems");
        }
    }
}
