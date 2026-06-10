using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace KidsSection.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKidsCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Rename existing categories ─────────────────────────────────
            migrationBuilder.UpdateData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Baby");

            migrationBuilder.UpdateData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Girls");

            migrationBuilder.UpdateData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Boys");

            migrationBuilder.UpdateData(
                table: "KidsCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Sleepwear");

            // ── Add new categories 5-8 (skip if already exist) ────────────
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM [KidsCategories] WHERE [Id] = 5)
                    INSERT INTO [KidsCategories] ([Id],[Name]) VALUES (5, 'Swimwear');
                IF NOT EXISTS (SELECT 1 FROM [KidsCategories] WHERE [Id] = 6)
                    INSERT INTO [KidsCategories] ([Id],[Name]) VALUES (6, 'Footwear');
                IF NOT EXISTS (SELECT 1 FROM [KidsCategories] WHERE [Id] = 7)
                    INSERT INTO [KidsCategories] ([Id],[Name]) VALUES (7, 'Accessories');
                IF NOT EXISTS (SELECT 1 FROM [KidsCategories] WHERE [Id] = 8)
                    INSERT INTO [KidsCategories] ([Id],[Name]) VALUES (8, 'Sale');
            ");

            // ── Add new products (skip if already exist) ───────────────────
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT [KidsCards] ON;
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 9)  INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (9,'Baby Girl Romper','https://images.unsplash.com/photo-1607227893600-9bc90f1ead82?w=400&q=80',18,'Cute floral baby romper',1,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 10) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (10,'Baby Boy Bodysuit','https://images.unsplash.com/photo-1574179618836-b7b81b2e1a4d?w=400&q=80',16,'Soft jersey baby bodysuit',1,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 11) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (11,'Baby Knit Set','https://images.unsplash.com/photo-1544367567-0f2fcb009e0b?w=400&q=80',22,'Cosy knitted 2-piece set',1,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 12) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (12,'Girls Sundress','https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=400&q=80',32,'Light summer sundress',2,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 13) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (13,'Girls Denim Skirt','https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=400&q=80',24,'Classic denim mini skirt',2,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 14) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (14,'Girls Puff Sleeve Top','https://images.unsplash.com/photo-1566174053879-31528523f8ae?w=400&q=80',20,'Trendy puff sleeve blouse',2,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 15) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (15,'Boys Polo Shirt','https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80',22,'Classic cotton polo shirt',3,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 16) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (16,'Boys Graphic Hoodie','https://images.unsplash.com/photo-1556821840-3a63f15732ce?w=400&q=80',30,'Cool graphic print hoodie',3,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 17) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (17,'Boys Chino Pants','https://images.unsplash.com/photo-1583744946564-b52ac1c389c8?w=400&q=80',26,'Smart casual chino trousers',3,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 18) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (18,'Boys Baseball Tee','https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400&q=80',16,'Sporty baseball-style tee',3,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 19) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (19,'Girls Star Pyjamas','https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=400&q=80',22,'Soft star-print pyjama set',4,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 20) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (20,'Boys Dino Pyjamas','https://images.unsplash.com/photo-1586105449897-20b5efeb3233?w=400&q=80',22,'Fun dinosaur pyjama set',4,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 21) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (21,'Baby Sleepsuit','https://images.unsplash.com/photo-1567578301402-c43d6dbef7de?w=400&q=80',18,'Warm fleece baby sleepsuit',4,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 22) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (22,'Girls Nightgown','https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400&q=80',20,'Cosy cotton nightgown',4,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 23) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (23,'Girls Swimsuit','https://images.unsplash.com/photo-1541099649105-f69ad21f3246?w=400&q=80',28,'Bright one-piece swimsuit',5,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 24) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (24,'Boys Swim Shorts','https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=400&q=80',20,'Quick-dry swim shorts',5,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 25) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (25,'Girls Bikini Set','https://images.unsplash.com/photo-1550639525-c97d455acf70?w=400&q=80',30,'Colourful two-piece bikini',5,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 26) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (26,'Kids Sneakers','https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&q=80',38,'Lightweight everyday sneakers',6,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 27) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (27,'Girls Sandals','https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=400&q=80',28,'Strappy summer sandals',6,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 28) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (28,'Boys Canvas Shoes','https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=400&q=80',32,'Classic canvas lace-up shoes',6,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 29) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (29,'Kids Rain Boots','https://images.unsplash.com/photo-1597248881519-db089b3a0508?w=400&q=80',34,'Waterproof rubber rain boots',6,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 30) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (30,'Kids School Backpack','https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=400&q=80',35,'Spacious padded school bag',7,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 31) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (31,'Girls Hair Bow Set','https://images.unsplash.com/photo-1553361371-9b22f78e8b1d?w=400&q=80',12,'Set of 5 colourful hair bows',7,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 32) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (32,'Kids Sun Hat','https://images.unsplash.com/photo-1485462537746-965f33f7f6a7?w=400&q=80',16,'Wide-brim UV protection hat',7,1,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 33) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (33,'Kids Sunglasses','https://images.unsplash.com/photo-1499002238440-d264edd596ec?w=400&q=80',14,'UV400 kids sunglasses',7,2,NULL,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 34) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (34,'Baby Patterned Onesie','https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80',20,'Adorable patterned baby onesie',8,1,30,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 35) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (35,'Girls Summer Dress','https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=400&q=80',35,'Light floral girls dress',8,2,25,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 36) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (36,'Boys Zip Hoodie','https://images.unsplash.com/photo-1471286174890-9c112ac6f609?w=400&q=80',32,'Warm zip-up boys hoodie',8,1,20,NULL);
                IF NOT EXISTS (SELECT 1 FROM [KidsCards] WHERE [Id] = 37) INSERT INTO [KidsCards] ([Id],[Title],[ImageUrl],[Price],[Description],[KidsCategoryId],[KidsProductTypeId],[DiscountPercent],[Color]) VALUES (37,'Kids Canvas Trainers','https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=400&q=80',40,'Classic canvas trainers',8,2,35,NULL);
                SET IDENTITY_INSERT [KidsCards] OFF;
            ");

            // ── Update existing products (fix categories + images) ─────────
            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Title", "ImageUrl", "Description", "KidsCategoryId" },
                values: new object[] { "Baby Striped Onesie", "https://images.unsplash.com/photo-1522771930-78848d9293e8?w=400&q=80", "Soft cotton striped onesie", 1 });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Title", "Description", "KidsCategoryId" },
                values: new object[] { "Girls Floral Dress", "Pretty summer floral dress", 2 });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Title", "Description" },
                values: new object[] { "Boys Denim Jeans", "Classic straight-fit jeans" });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Title", "Description", "KidsCategoryId" },
                values: new object[] { "Boys Summer Shorts", "Light cotton summer shorts", 3 });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Title", "ImageUrl", "Description", "KidsCategoryId" },
                values: new object[] { "Baby Rainbow Bodysuit", "https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?w=400&q=80", "Colourful rainbow bodysuit", 1 });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Title", "Description" },
                values: new object[] { "Girls Princess Dress", "Elegant princess party dress" });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Title", "Description", "KidsCategoryId" },
                values: new object[] { "Boys Cargo Shorts", "Practical multi-pocket shorts", 3 });

            migrationBuilder.UpdateData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Title", "Description", "KidsCategoryId" },
                values: new object[] { "Girls Leggings", "Comfortable stretch leggings", 2 });

            // ── Add new products handled above via Sql() ──────────────────
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "KidsCards", keyColumn: "Id", keyValues: new object[] { 9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37 });

            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 1, columns: new[] { "Title","ImageUrl","Description","KidsCategoryId" }, values: new object[] { "Striped T-Shirt","https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80","Colorful striped t-shirt",1 });
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 2, columns: new[] { "Title","Description","KidsCategoryId" }, values: new object[] { "Floral Dress","Pretty floral dress",2 });
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 4, columns: new[] { "Title","Description","KidsCategoryId" }, values: new object[] { "Summer Shorts","Light summer shorts",4 });
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 5, columns: new[] { "Title","ImageUrl","Description","KidsCategoryId" }, values: new object[] { "Rainbow Hoodie","https://images.unsplash.com/photo-1556821840-3a63f15732ce?w=400&q=80","Fun rainbow hoodie",1 });
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 7, columns: new[] { "Title","Description","KidsCategoryId" }, values: new object[] { "Cargo Shorts","Practical cargo shorts",4 });
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 8, columns: new[] { "Title","Description","KidsCategoryId" }, values: new object[] { "Leggings","Comfortable leggings",3 });

            migrationBuilder.DeleteData(table: "KidsCategories", keyColumn: "Id", keyValues: new object[] { 5,6,7,8 });
            migrationBuilder.UpdateData(table: "KidsCategories", keyColumn: "Id", keyValue: 1, column: "Name", value: "T-Shirts");
            migrationBuilder.UpdateData(table: "KidsCategories", keyColumn: "Id", keyValue: 2, column: "Name", value: "Dresses");
            migrationBuilder.UpdateData(table: "KidsCategories", keyColumn: "Id", keyValue: 3, column: "Name", value: "Jeans");
            migrationBuilder.UpdateData(table: "KidsCategories", keyColumn: "Id", keyValue: 4, column: "Name", value: "Shorts");
        }
    }
}
