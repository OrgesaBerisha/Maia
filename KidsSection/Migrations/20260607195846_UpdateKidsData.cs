using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KidsSection.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKidsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ensure all required categories exist (upsert with identity insert)
            migrationBuilder.Sql("SET IDENTITY_INSERT KidsCategories ON");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM KidsCategories WHERE Id = 1) INSERT INTO KidsCategories (Id, Name) VALUES (1, 'Baby') ELSE UPDATE KidsCategories SET Name = 'Baby' WHERE Id = 1");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM KidsCategories WHERE Id = 2) INSERT INTO KidsCategories (Id, Name) VALUES (2, 'Girls') ELSE UPDATE KidsCategories SET Name = 'Girls' WHERE Id = 2");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM KidsCategories WHERE Id = 3) INSERT INTO KidsCategories (Id, Name) VALUES (3, 'Boys') ELSE UPDATE KidsCategories SET Name = 'Boys' WHERE Id = 3");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM KidsCategories WHERE Id = 4) INSERT INTO KidsCategories (Id, Name) VALUES (4, 'Sleepwear') ELSE UPDATE KidsCategories SET Name = 'Sleepwear' WHERE Id = 4");
            migrationBuilder.Sql("SET IDENTITY_INSERT KidsCategories OFF");

            // Insert base products (IDs 1-8) using SQL to handle empty table
            migrationBuilder.Sql("SET IDENTITY_INSERT KidsCards ON");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM KidsCards WHERE Id = 1) INSERT INTO KidsCards (Id, Title, Description, ImageUrl, Price, KidsCategoryId, KidsProductTypeId, Color, DiscountPercent) VALUES (1, 'Baby Striped Onesie', 'Soft cotton striped onesie', 'https://images.unsplash.com/photo-1522771930-78848d9293e8?w=400&q=80', 20.0, 1, 1, NULL, NULL);
                IF NOT EXISTS (SELECT 1 FROM KidsCards WHERE Id = 2) INSERT INTO KidsCards (Id, Title, Description, ImageUrl, Price, KidsCategoryId, KidsProductTypeId, Color, DiscountPercent) VALUES (2, 'Girls Floral Dress', 'Pretty summer floral dress', 'https://images.unsplash.com/photo-1518831959646-742c3a14ebf7?w=400&q=80', 28.0, 2, 1, NULL, NULL);
                IF NOT EXISTS (SELECT 1 FROM KidsCards WHERE Id = 3) INSERT INTO KidsCards (Id, Title, Description, ImageUrl, Price, KidsCategoryId, KidsProductTypeId, Color, DiscountPercent) VALUES (3, 'Boys Denim Jeans', 'Classic straight-fit jeans', 'https://images.unsplash.com/photo-1598554747436-c9293d6a588f?w=400&q=80', 28.0, 3, 1, NULL, NULL);
                IF NOT EXISTS (SELECT 1 FROM KidsCards WHERE Id = 4) INSERT INTO KidsCards (Id, Title, Description, ImageUrl, Price, KidsCategoryId, KidsProductTypeId, Color, DiscountPercent) VALUES (4, 'Boys Summer Shorts', 'Light cotton summer shorts', 'https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?w=400&q=80', 18.0, 3, 1, NULL, NULL);
                IF NOT EXISTS (SELECT 1 FROM KidsCards WHERE Id = 5) INSERT INTO KidsCards (Id, Title, Description, ImageUrl, Price, KidsCategoryId, KidsProductTypeId, Color, DiscountPercent) VALUES (5, 'Baby Rainbow Bodysuit', 'Colourful rainbow bodysuit', 'https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?w=400&q=80', 14.0, 1, 1, NULL, NULL);
                IF NOT EXISTS (SELECT 1 FROM KidsCards WHERE Id = 6) INSERT INTO KidsCards (Id, Title, Description, ImageUrl, Price, KidsCategoryId, KidsProductTypeId, Color, DiscountPercent) VALUES (6, 'Girls Princess Dress', 'Elegant princess party dress', 'https://images.unsplash.com/photo-1518831959646-742c3a14ebf7?w=400&q=80', 35.0, 2, 1, NULL, NULL);
                IF NOT EXISTS (SELECT 1 FROM KidsCards WHERE Id = 7) INSERT INTO KidsCards (Id, Title, Description, ImageUrl, Price, KidsCategoryId, KidsProductTypeId, Color, DiscountPercent) VALUES (7, 'Boys Cargo Shorts', 'Practical multi-pocket shorts', 'https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?w=400&q=80', 20.0, 3, 1, NULL, NULL);
                IF NOT EXISTS (SELECT 1 FROM KidsCards WHERE Id = 8) INSERT INTO KidsCards (Id, Title, Description, ImageUrl, Price, KidsCategoryId, KidsProductTypeId, Color, DiscountPercent) VALUES (8, 'Girls Leggings', 'Comfortable stretch leggings', 'https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=400&q=80', 16.0, 2, 1, NULL, NULL);
            ");
            migrationBuilder.Sql("SET IDENTITY_INSERT KidsCards OFF");

            migrationBuilder.InsertData(
                table: "KidsCards",
                columns: new[] { "Id", "Color", "Description", "DiscountPercent", "ImageUrl", "KidsCategoryId", "KidsProductTypeId", "Price", "Title" },
                values: new object[,]
                {
                    { 9, null, "Cute floral baby romper", null, "https://images.unsplash.com/photo-1607227893600-9bc90f1ead82?w=400&q=80", 1, 2, 18m, "Baby Girl Romper" },
                    { 10, null, "Soft jersey baby bodysuit", null, "https://images.unsplash.com/photo-1574179618836-b7b81b2e1a4d?w=400&q=80", 1, 1, 16m, "Baby Boy Bodysuit" },
                    { 11, null, "Cosy knitted 2-piece set", null, "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b?w=400&q=80", 1, 2, 22m, "Baby Knit Set" },
                    { 12, null, "Light summer sundress", null, "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=400&q=80", 2, 2, 32m, "Girls Sundress" },
                    { 13, null, "Classic denim mini skirt", null, "https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=400&q=80", 2, 2, 24m, "Girls Denim Skirt" },
                    { 14, null, "Trendy puff sleeve blouse", null, "https://images.unsplash.com/photo-1566174053879-31528523f8ae?w=400&q=80", 2, 2, 20m, "Girls Puff Sleeve Top" },
                    { 15, null, "Classic cotton polo shirt", null, "https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80", 3, 1, 22m, "Boys Polo Shirt" },
                    { 16, null, "Cool graphic print hoodie", null, "https://images.unsplash.com/photo-1556821840-3a63f15732ce?w=400&q=80", 3, 1, 30m, "Boys Graphic Hoodie" },
                    { 17, null, "Smart casual chino trousers", null, "https://images.unsplash.com/photo-1583744946564-b52ac1c389c8?w=400&q=80", 3, 1, 26m, "Boys Chino Pants" },
                    { 18, null, "Sporty baseball-style tee", null, "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400&q=80", 3, 1, 16m, "Boys Baseball Tee" },
                    { 19, null, "Soft star-print pyjama set", null, "https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=400&q=80", 4, 2, 22m, "Girls Star Pyjamas" },
                    { 20, null, "Fun dinosaur pyjama set", null, "https://images.unsplash.com/photo-1586105449897-20b5efeb3233?w=400&q=80", 4, 1, 22m, "Boys Dino Pyjamas" },
                    { 21, null, "Warm fleece baby sleepsuit", null, "https://images.unsplash.com/photo-1567578301402-c43d6dbef7de?w=400&q=80", 4, 1, 18m, "Baby Sleepsuit" },
                    { 22, null, "Cosy cotton nightgown", null, "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400&q=80", 4, 2, 20m, "Girls Nightgown" }
                });

            migrationBuilder.Sql("SET IDENTITY_INSERT KidsCategories ON");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM KidsCategories WHERE Id = 5) INSERT INTO KidsCategories (Id, Name) VALUES (5, 'Swimwear') ELSE UPDATE KidsCategories SET Name = 'Swimwear' WHERE Id = 5");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM KidsCategories WHERE Id = 6) INSERT INTO KidsCategories (Id, Name) VALUES (6, 'Footwear') ELSE UPDATE KidsCategories SET Name = 'Footwear' WHERE Id = 6");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM KidsCategories WHERE Id = 7) INSERT INTO KidsCategories (Id, Name) VALUES (7, 'Accessories') ELSE UPDATE KidsCategories SET Name = 'Accessories' WHERE Id = 7");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM KidsCategories WHERE Id = 8) INSERT INTO KidsCategories (Id, Name) VALUES (8, 'Sale') ELSE UPDATE KidsCategories SET Name = 'Sale' WHERE Id = 8");
            migrationBuilder.Sql("SET IDENTITY_INSERT KidsCategories OFF");

            migrationBuilder.InsertData(
                table: "KidsCards",
                columns: new[] { "Id", "Color", "Description", "DiscountPercent", "ImageUrl", "KidsCategoryId", "KidsProductTypeId", "Price", "Title" },
                values: new object[,]
                {
                    { 23, null, "Bright one-piece swimsuit", null, "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?w=400&q=80", 5, 2, 28m, "Girls Swimsuit" },
                    { 24, null, "Quick-dry swim shorts", null, "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=400&q=80", 5, 1, 20m, "Boys Swim Shorts" },
                    { 25, null, "Colourful two-piece bikini", null, "https://images.unsplash.com/photo-1550639525-c97d455acf70?w=400&q=80", 5, 2, 30m, "Girls Bikini Set" },
                    { 26, null, "Lightweight everyday sneakers", null, "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&q=80", 6, 1, 38m, "Kids Sneakers" },
                    { 27, null, "Strappy summer sandals", null, "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=400&q=80", 6, 2, 28m, "Girls Sandals" },
                    { 28, null, "Classic canvas lace-up shoes", null, "https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=400&q=80", 6, 1, 32m, "Boys Canvas Shoes" },
                    { 29, null, "Waterproof rubber rain boots", null, "https://images.unsplash.com/photo-1597248881519-db089b3a0508?w=400&q=80", 6, 2, 34m, "Kids Rain Boots" },
                    { 30, null, "Spacious padded school bag", null, "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=400&q=80", 7, 1, 35m, "Kids School Backpack" },
                    { 31, null, "Set of 5 colourful hair bows", null, "https://images.unsplash.com/photo-1553361371-9b22f78e8b1d?w=400&q=80", 7, 2, 12m, "Girls Hair Bow Set" },
                    { 32, null, "Wide-brim UV protection hat", null, "https://images.unsplash.com/photo-1485462537746-965f33f7f6a7?w=400&q=80", 7, 1, 16m, "Kids Sun Hat" },
                    { 33, null, "UV400 kids sunglasses", null, "https://images.unsplash.com/photo-1499002238440-d264edd596ec?w=400&q=80", 7, 2, 14m, "Kids Sunglasses" },
                    { 34, null, "Adorable patterned baby onesie", 30, "https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80", 8, 1, 20m, "Baby Patterned Onesie" },
                    { 35, null, "Light floral girls dress", 25, "https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=400&q=80", 8, 2, 35m, "Girls Summer Dress" },
                    { 36, null, "Warm zip-up boys hoodie", 20, "https://images.unsplash.com/photo-1471286174890-9c112ac6f609?w=400&q=80", 8, 1, 32m, "Boys Zip Hoodie" },
                    { 37, null, "Classic canvas trainers", 35, "https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=400&q=80", 8, 2, 40m, "Kids Canvas Trainers" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Title" },
                values: new object[] { "Colorful striped t-shirt", "https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80", "Striped T-Shirt" });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Price", "Title" },
                values: new object[] { "Pretty floral dress", 18m, "Floral Dress" });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Price", "Title" },
                values: new object[] { "Classic denim jeans", 22m, "Denim Jeans" });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "KidsCategoryId", "KidsProductTypeId", "Price", "Title" },
                values: new object[] { "Light summer shorts", 4, 2, 14m, "Summer Shorts" });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "ImageUrl", "KidsProductTypeId", "Price", "Title" },
                values: new object[] { "Fun rainbow hoodie", "https://images.unsplash.com/photo-1556821840-3a63f15732ce?w=400&q=80", 1, 20m, "Rainbow Hoodie" });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Price", "Title" },
                values: new object[] { "Lovely princess dress", 25m, "Princess Dress" });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "KidsCategoryId", "Price", "Title" },
                values: new object[] { "Practical cargo shorts", 4, 16m, "Cargo Shorts" });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "KidsCategoryId", "Title" },
                values: new object[] { "Comfortable leggings", 3, "Leggings" });

            migrationBuilder.UpdateData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "T-Shirts");

            migrationBuilder.UpdateData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Dresses");

            migrationBuilder.UpdateData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Jeans");

            migrationBuilder.UpdateData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Shorts");
        }
    }
}
