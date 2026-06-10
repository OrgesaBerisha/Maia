using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidsSection.Migrations
{
    public partial class AddKidsColors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Assign colors based on keywords in product titles
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Black'  WHERE Color IS NULL AND (Title LIKE '%Black%' OR Title LIKE '%Velvet%' OR Title LIKE '%Dark%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'White'  WHERE Color IS NULL AND (Title LIKE '%White%' OR Title LIKE '%Ivory%' OR Title LIKE '%Silver%' OR Title LIKE '%Cream%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Navy'   WHERE Color IS NULL AND (Title LIKE '%Navy%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Blue'   WHERE Color IS NULL AND (Title LIKE '%Blue%' OR Title LIKE '%Denim%' OR Title LIKE '%Jeans%' OR Title LIKE '%Teal%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Pink'   WHERE Color IS NULL AND (Title LIKE '%Pink%' OR Title LIKE '%Floral%' OR Title LIKE '%Ruffle%' OR Title LIKE '%Tie-Dye%' OR Title LIKE '%Smocked%' OR Title LIKE '%Bikini%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Grey'   WHERE Color IS NULL AND (Title LIKE '%Grey%' OR Title LIKE '%Gray%' OR Title LIKE '%Fleece%' OR Title LIKE '%Sweatshirt%' OR Title LIKE '%Sweatpants%' OR Title LIKE '%Hoodie%' OR Title LIKE '%Joggers%' OR Title LIKE '%Sweat%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Green'  WHERE Color IS NULL AND (Title LIKE '%Green%' OR Title LIKE '%Camo%' OR Title LIKE '%Utility%' OR Title LIKE '%Cargo%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Yellow' WHERE Color IS NULL AND (Title LIKE '%Yellow%' OR Title LIKE '%Neon%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Red'    WHERE Color IS NULL AND (Title LIKE '%Red%' OR Title LIKE '%Shark%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Beige'  WHERE Color IS NULL AND (Title LIKE '%Beige%' OR Title LIKE '%Linen%' OR Title LIKE '%Knit%' OR Title LIKE '%Cardigan%' OR Title LIKE '%Ribbed%' OR Title LIKE '%Quilted%' OR Title LIKE '%Velour%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Blue'   WHERE Color IS NULL AND (Title LIKE '%Striped%' OR Title LIKE '%Polo%' OR Title LIKE '%Board Short%' OR Title LIKE '%Track%' OR Title LIKE '%Tech%' OR Title LIKE '%Rash Guard%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'White'  WHERE Color IS NULL AND (Title LIKE '%Romper%' OR Title LIKE '%Playsuit%' OR Title LIKE '%Bodysuit%' OR Title LIKE '%Body%' OR Title LIKE '%Onesie%' OR Title LIKE '%Wrap%' OR Title LIKE '%Sleepsuit%' OR Title LIKE '%Zip Sleeve%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Pink'   WHERE Color IS NULL AND (Title LIKE '%Dress%' OR Title LIKE '%Skirt%' OR Title LIKE '%Blouse%' OR Title LIKE '%Jumpsuit%' OR Title LIKE '%Sequin%' OR Title LIKE '%Sundress%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Blue'   WHERE Color IS NULL AND (Title LIKE '%Trunks%' OR Title LIKE '%Swim Short%' OR Title LIKE '%Board%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Black'  WHERE Color IS NULL AND (Title LIKE '%Boot%' OR Title LIKE '%Loafer%' OR Title LIKE '%Classic Shoe%' OR Title LIKE '%Ankle Boot%' OR Title LIKE '%Mule%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Beige'  WHERE Color IS NULL AND (Title LIKE '%Sandal%' OR Title LIKE '%Ballet%' OR Title LIKE '%Mary Jane%' OR Title LIKE '%Flower%' OR Title LIKE '%Trainer%' OR Title LIKE '%Sneaker%' OR Title LIKE '%Slip-On%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Brown'  WHERE Color IS NULL AND (Title LIKE '%Leather%' OR Title LIKE '%Chelsea%' OR Title LIKE '%Belt%' OR Title LIKE '%Wallet%' OR Title LIKE '%Bag%' OR Title LIKE '%Backpack%' OR Title LIKE '%Purse%' OR Title LIKE '%Crossbody%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Pink'   WHERE Color IS NULL AND (Title LIKE '%Bracelet%' OR Title LIKE '%Necklace%' OR Title LIKE '%Charm%' OR Title LIKE '%Scrunchie%' OR Title LIKE '%Hair%' OR Title LIKE '%Headband%')");
            migrationBuilder.Sql("UPDATE KidsCards SET Color = 'Beige'  WHERE Color IS NULL AND (Title LIKE '%Cap%' OR Title LIKE '%Hat%' OR Title LIKE '%Beanie%' OR Title LIKE '%Scarf%' OR Title LIKE '%Flat Cap%' OR Title LIKE '%Bobble%')");

            // For any remaining products still without a color, distribute evenly by ID
            migrationBuilder.Sql(@"
                UPDATE KidsCards SET Color =
                    CASE (Id % 11)
                        WHEN 0  THEN 'Black'
                        WHEN 1  THEN 'White'
                        WHEN 2  THEN 'Blue'
                        WHEN 3  THEN 'Pink'
                        WHEN 4  THEN 'Navy'
                        WHEN 5  THEN 'Grey'
                        WHEN 6  THEN 'Beige'
                        WHEN 7  THEN 'Brown'
                        WHEN 8  THEN 'Green'
                        WHEN 9  THEN 'Red'
                        WHEN 10 THEN 'Yellow'
                    END
                WHERE Color IS NULL
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE KidsCards SET Color = NULL");
        }
    }
}
