using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace KidsSection.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSleepwearAddImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove Sleepwear products before deleting category (FK safety)
            migrationBuilder.DeleteData(table: "KidsCards", keyColumn: "Id", keyValues: new object[] { 19, 20, 21, 22 });
            migrationBuilder.DeleteData(table: "KidsCategories", keyColumn: "Id", keyValue: 4);

            // Update Swimwear image URLs (IDs 23-25)
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 23, column: "ImageUrl", value: "https://image.hm.com/assets/hm/48/aa/48aa0782a07fd5d57fb59a2743db10904a951cc5.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 24, column: "ImageUrl", value: "https://image.hm.com/assets/hm/41/96/4196605a4cc46336db2af905686f46f34c0c4f2a.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 25, column: "ImageUrl", value: "https://image.hm.com/assets/hm/85/e2/85e2a4ca8980000abb7e5909d3030c8640e4e8cd.jpg?imwidth=1260");

            // Update Footwear image URLs (IDs 26-29)
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 26, column: "ImageUrl", value: "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dweff593fb/pim-static/NI/13263283/13263283_CrmeDePche_1332588_008.jpg?sw=450&sh=600&strip=false");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 27, column: "ImageUrl", value: "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dw6add5a19/pim-static/NI/13263283/13263283_Black_1332346_003.jpg?sw=450&sh=600&strip=false");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 28, column: "ImageUrl", value: "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dw508a6105/pim-static/NI/13263283/13263283_BegoniaPink_1332350_008.jpg?sw=450&sh=600&strip=false");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 29, column: "ImageUrl", value: "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dwa2c05bb6/pim-static/NI/13263283/13263283_CrmeDePche_1332350_003.jpg?sw=450&sh=600&strip=false");

            // Update Accessories image URLs (IDs 30-33)
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 30, column: "ImageUrl", value: "https://static.zara.net/assets/public/a97a/daec/2474439daa46/a30259e1425f/11442730400-e1/11442730400-e1.jpg?ts=1778828996352&w=184");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 31, column: "ImageUrl", value: "https://static.zara.net/assets/public/691d/90ff/65ab41a88c3d/0b7d18d197ff/11226730050-e1/11226730050-e1.jpg?ts=1776779422259&w=184");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 32, column: "ImageUrl", value: "https://static.zara.net/assets/public/41c0/5614/22e04510a756/bf384c42b0e2/11104730002-e1/11104730002-e1.jpg?ts=1764173531641&w=184");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 33, column: "ImageUrl", value: "https://static.zara.net/assets/public/cc8b/3a8c/26b84cb58fc2/688ae73f28a4/11113830800-e1/11113830800-e1.jpg?ts=1778773135501&w=184");

            // Update Sale image URLs (IDs 34-37)
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 34, column: "ImageUrl", value: "https://static.zara.net/assets/public/9550/feb7/981e47c1a61e/4123474202cf/04676601800-e1/04676601800-e1.jpg?ts=1765368970400&w=133");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 35, column: "ImageUrl", value: "https://static.zara.net/assets/public/8c99/e2da/3e1d4b64ad4d/4b31d8f4b115/00722731712300-p/00722731712300-p.jpg?ts=1768471273341&w=133");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 36, column: "ImageUrl", value: "https://static.zara.net/assets/public/4264/541d/951d47208ccf/ffc7b1b1fba5/05431330526-p/05431330526-p.jpg?ts=1774437078246&w=133");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 37, column: "ImageUrl", value: "https://static.zara.net/assets/public/b02f/0a19/318b43898975/a30f73ae8bff/03854690804-e1/03854690804-e1.jpg?ts=1764607154306&w=133");

            // Ensure Baby/Girls/Boys products have H&M image URLs.
            // Products 1 & 5 are re-set here because migration 20260607000001 overwrites their
            // ImageUrl with Unsplash placeholders when it runs AFTER 20260609000001 (which had
            // already applied the H&M URLs). For repositories where the ordering was correct
            // (20260607000001 before 20260609000001) these are no-ops (same values).
            // Products 9-18 were inserted by 20260607000001 with Unsplash URLs; if 20260609000001
            // ran before that migration existed those rows didn't exist yet so it skipped them.
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 1,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/8c/04/8c04c5efaa10c9ce45aa90a0736a3f655380e82d.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 5,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/42/f4/42f4c83ce439dacd1f7139ef729670df582b0fe0.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 9,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/c9/e0/c9e006d06dccb847daaac634ec3acac6c17807f5.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 10, column: "ImageUrl", value: "https://image.hm.com/assets/hm/d8/1e/d81e720a2bb94a1f788417943316de657e36d28a.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 11, column: "ImageUrl", value: "https://image.hm.com/assets/hm/1b/68/1b68a32d8c0c25a3570a41bf5669d2924d36ed4c.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 12, column: "ImageUrl", value: "https://image.hm.com/assets/hm/9f/0e/9f0e60edfbc8d7676ff7c7d1efd9c91505c7644a.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 13, column: "ImageUrl", value: "https://image.hm.com/assets/hm/e6/10/e61010dcc4134f59cf924bd16d37057ca1341391.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 14, column: "ImageUrl", value: "https://image.hm.com/assets/hm/c6/38/c638eb92d0419205d9c79c2bfc03afa13785b5d0.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 15, column: "ImageUrl", value: "https://image.hm.com/assets/hm/b6/6e/b66eee7aa009d6f0a0e72b112bc4f99ea6af8d05.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 16, column: "ImageUrl", value: "https://image.hm.com/assets/hm/fa/68/fa6891f65019c07a31f6fe616ccf8e5e53dc26b0.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 17, column: "ImageUrl", value: "https://image.hm.com/assets/hm/30/31/3031eb8ab1421f329731d54b8b35b15f1c8b6711.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 18, column: "ImageUrl", value: "https://image.hm.com/assets/hm/0d/2a/0d2a9d03b5aca7b15f680726b3186520bd1513b6.jpg?imwidth=1260");

            // Insert new Swimwear products (IDs 84-103)
            migrationBuilder.InsertData(
                table: "KidsCards",
                columns: new[] { "Id", "Title", "ImageUrl", "Price", "Description", "KidsCategoryId", "KidsProductTypeId", "DiscountPercent", "Color" },
                values: new object[,]
                {
                    { 84,  "Girls Tie-Dye Swimsuit",     "https://image.hm.com/assets/hm/1c/0d/1c0dcb2824bdce6899872c79196bd4b86c4bca9c.jpg?imwidth=1260", 26m, "Vibrant tie-dye one-piece swimsuit",  5, 2, null, null },
                    { 85,  "Boys Striped Swim Trunks",   "https://image.hm.com/assets/hm/00/f3/00f3b9957973f25a5b7d753a5fb238e0fa08edb9.jpg?imwidth=1260", 22m, "Classic striped swim trunks",         5, 1, null, null },
                    { 86,  "Girls Rash Guard Set",       "https://image.hm.com/assets/hm/9e/e7/9ee77b03f142149ff85c7cf8110824d54e65dde7.jpg?imwidth=1260", 30m, "UV-protective rash guard set",        5, 2, null, null },
                    { 87,  "Girls Floral Two-Piece",     "https://image.hm.com/assets/hm/ad/da/adda5127cc738ddb93faebc86f34b3c18ae382e1.jpg",              28m, "Pretty floral bikini two-piece",       5, 2, null, null },
                    { 88,  "Boys Shark Swim Shorts",     "https://image.hm.com/assets/hm/21/67/216743203ff043215e7e2b6b85a7da3284aa31ba.jpg?imwidth=1260", 24m, "Fun shark-print swim shorts",         5, 1, null, null },
                    { 89,  "Boys Blue Swim Shorts",      "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dwb39aa9b2/pim-static/NI/13253245/13253245_AtomicBlue_003.jpg?sw=450&sh=600&strip=false", 22m, "Bright blue quick-dry swim shorts",   5, 1, null, null },
                    { 90,  "Girls UV Swimsuit",          "https://image.hm.com/assets/hm/65/40/6540a32c6967cbad5932853c7c0e7b5b35a2bab2.jpg?imwidth=1260", 28m, "UPF 50+ sun-protective swimsuit",     5, 2, null, null },
                    { 91,  "Girls Polka Dot Bikini",     "https://image.hm.com/assets/hm/85/8f/858fb197d69496b553b2c07415f84e72812e264d.jpg?imwidth=1260", 30m, "Playful polka dot bikini set",        5, 2, null, null },
                    { 92,  "Boys Neon Swim Shorts",      "https://image.hm.com/assets/hm/95/f1/95f158948e6bf798b84375d5c667d630fe40b4cc.jpg?imwidth=1260", 22m, "Bright neon quick-dry shorts",        5, 1, null, null },
                    { 93,  "Girls Navy Swimsuit",        "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dwe7134f1e/pim-static/NI/13253322/13253322_NavyBlazer_003.jpg?sw=450&sh=600&strip=false", 26m, "Classic navy one-piece swimsuit",     5, 2, null, null },
                    { 94,  "Boys Tropical Board Shorts", "https://image.hm.com/assets/hm/88/86/88861cc5fe5e15253a493829d1d476b5754f9b58.jpg?imwidth=1260", 26m, "Vibrant tropical-print board shorts", 5, 1, null, null },
                    { 95,  "Girls Ruffle Bikini",        "https://image.hm.com/assets/hm/e4/59/e4598b2a2c745ad8a68cb0a9e5fa286a939f837e.jpg?imwidth=1260", 32m, "Pretty ruffle-trim bikini set",       5, 2, null, null },
                    { 96,  "Girls Grey Swimsuit",        "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dwf7e4800d/pim-static/NI/13253320/13253320_ChateauGray_007.jpg?sw=450&sh=600&strip=false", 28m, "Chic grey one-piece swimsuit",        5, 2, null, null },
                    { 97,  "Boys Camo Swim Shorts",      "https://image.hm.com/assets/hm/35/cc/35cc54031d8ae15aa6fde169ed1315514cdcbdbb.jpg?imwidth=1260", 24m, "Cool camouflage swim shorts",         5, 1, null, null },
                    { 98,  "Girls White Bikini",         "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dwcf87c070/pim-static/NI/13253242/13253242_BrightWhite_003.jpg?sw=450&sh=600&strip=false", 28m, "Fresh white two-piece bikini",        5, 2, null, null },
                    { 99,  "Boys Patterned Swim Trunks", "https://image.hm.com/assets/hm/7a/a5/7aa5c64f0c727c1285a90cbcbf5d1a2f824aafdd.jpg?imwidth=1260", 22m, "Bold patterned swim trunks",          5, 1, null, null },
                    { 100, "Girls Black Bikini",         "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dw2677595d/pim-static/NI/13253244/13253244_Black_003.jpg?sw=450&sh=600&strip=false", 28m, "Sleek black two-piece bikini",        5, 2, null, null },
                    { 101, "Girls Teal Bikini",          "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dw5900bf23/pim-static/NI/13253322/13253322_LimpetShell_003.jpg?sw=450&sh=600&strip=false", 26m, "Vibrant teal two-piece bikini",       5, 2, null, null },
                    { 102, "Boys Blue Swim Shorts Set",  "https://image.hm.com/assets/hm/b9/eb/b9ebe53004b9dda52bf88261e73e620e4bbb1ec9.jpg?imwidth=1260", 22m, "Navy blue swim shorts",               5, 1, null, null },
                    { 103, "Girls Yellow Swimwear",      "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dw06f4f2bf/pim-static/NI/13253243/13253243_SafetyYellow_008.jpg?sw=450&sh=600&strip=false", 24m, "Bright yellow swimwear set",          5, 2, null, null },
                });

            // Insert new Footwear products (IDs 104-119)
            migrationBuilder.InsertData(
                table: "KidsCards",
                columns: new[] { "Id", "Title", "ImageUrl", "Price", "Description", "KidsCategoryId", "KidsProductTypeId", "DiscountPercent", "Color" },
                values: new object[,]
                {
                    { 104, "Girls Silver Shoes",       "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dw2348eb66/pim-static/NI/13215550/13215550_Silver_1258164_008.jpg?sw=450&sh=600&strip=false", 34m, "Sparkly silver occasion shoes",     6, 2, null, null },
                    { 105, "Girls Pink Ballet Flats",  "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dw79331750/pim-static/NI/13215550/13215550_Pirouette_1258164_008.jpg?sw=450&sh=600&strip=false", 30m, "Soft pink flat ballet shoes",       6, 2, null, null },
                    { 106, "Girls Flower Sandals",     "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dw325f98d7/pim-static/NI/13263283/13263283_Nosegay_1332588_007.jpg?sw=450&sh=600&strip=false", 28m, "Pretty floral-strap sandals",       6, 2, null, null },
                    { 107, "Boys Dark Sneakers",       "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dw6828de1d/pim-static/NI/13263283/13263283_Kalamata_1332346_008.jpg?sw=450&sh=600&strip=false", 36m, "Sporty dark-olive sneakers",        6, 1, null, null },
                    { 108, "Boys Leather Boots",       "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dwfdc974fe/pim-static/NI/13231208/13231208_BitterChocolate_008.jpg?sw=450&sh=600&strip=false", 42m, "Classic leather ankle boots",       6, 1, null, null },
                    { 109, "Kids Chelsea Boots",       "https://www.nameit.com/dw/image/v2/BDTC_PRD/on/demandware.static/-/Sites-pim-catalog/default/dw733c04b6/pim-static/NI/13248115/13248115_BlackCoffee_003.jpg?sw=450&sh=600&strip=false", 48m, "Timeless black Chelsea boots",      6, 1, null, null },
                    { 110, "Kids White Trainers",      "https://static.zara.net/assets/public/9358/c30c/9b2041acb10a/b687b802c46f/14440830600-e2/14440830600-e2.jpg?ts=1780410309892&w=368", 36m, "Clean white everyday trainers",     6, 1, null, null },
                    { 111, "Kids Loafer Shoes",        "https://static.zara.net/assets/public/e331/3c48/5a0d44598509/b68f2329d52e/14406830002-032-p/14406830002-032-p.jpg?ts=1780648044489&w=433", 38m, "Smart leather loafer shoes",        6, 2, null, null },
                    { 112, "Girls Slip-On Shoes",      "https://static.zara.net/assets/public/3eee/2b6b/28b84ceba96e/0cd931a9c71e/14432630001031-p/14432630001031-p.jpg?ts=1776253651151&w=433", 32m, "Easy slip-on casual shoes",         6, 2, null, null },
                    { 113, "Kids Leather Sandals",     "https://static.zara.net/assets/public/4968/c952/7d424f3da835/7a85384a015f/14406830002-ult22/14406830002-ult22.jpg?ts=1779291114073&w=433", 34m, "Genuine leather summer sandals",    6, 1, null, null },
                    { 114, "Boys Classic Shoes",       "https://static.zara.net/assets/public/34d3/77dc/cb244cdfaca2/8464956d9199/14414730001-ult22/14414730001-ult22.jpg?ts=1769439370632&w=433", 40m, "Smart casual lace-up shoes",        6, 1, null, null },
                    { 115, "Girls Platform Sneakers",  "https://static.zara.net/assets/public/eddd/b74c/b6d7406fad84/a3b3c5d9c8b1/14506730107034-a1/14506730107034-a1.jpg?ts=1773764178966&w=433", 38m, "Trendy platform sole sneakers",     6, 2, null, null },
                    { 116, "Kids Ankle Boots",         "https://static.zara.net/assets/public/8d31/268b/7be6484aa0ef/b8255f0bb675/16537730002-ult40/16537730002-ult40.jpg?ts=1773832894977&w=284", 44m, "Durable lace-up ankle boots",       6, 1, null, null },
                    { 117, "Girls Mary Janes",         "https://static.zara.net/assets/public/9c8a/e38f/c4764ba58ec5/1521c85baba9/16613830612021-p/16613830612021-p.jpg?ts=1779954006356&w=284", 34m, "Classic strap Mary Jane shoes",     6, 2, null, null },
                    { 118, "Boys Sport Sandals",       "https://static.zara.net/assets/public/41ae/592b/2e81437eada1/ce85c5f19ca5/16317730001022-a1/16317730001022-a1.jpg?ts=1779954007322&w=284", 30m, "Adjustable sport sandals",          6, 1, null, null },
                    { 119, "Kids Slip-On Mules",       "https://static.zara.net/assets/public/14fb/cac9/0e3f4757b60b/84f8c287a84c/16637730050023-p/16637730050023-p.jpg?ts=1779954006896&w=184", 32m, "Easy-on slip-on mule shoes",        6, 2, null, null },
                });

            // Insert new Accessories products (IDs 120-140)
            migrationBuilder.InsertData(
                table: "KidsCards",
                columns: new[] { "Id", "Title", "ImageUrl", "Price", "Description", "KidsCategoryId", "KidsProductTypeId", "DiscountPercent", "Color" },
                values: new object[,]
                {
                    { 120, "Kids Coin Purse",        "https://static.zara.net/assets/public/bb86/5fa3/eb3445e88afb/cca5dab00a59/04786558067500-p/04786558067500-p.jpg?ts=1775641852703&w=184", 14m, "Cute zip-up kids coin purse",           7, 2, null, null },
                    { 121, "Kids Hair Clip Set",     "https://static.zara.net/assets/public/5971/d8e3/05c5432797cb/6957e4c9003b/06061517061500-p/06061517061500-p.jpg?ts=1776266315739&w=184", 12m, "Set of colourful hair clips",            7, 2, null, null },
                    { 122, "Girls Floral Headband",  "https://static.zara.net/assets/public/bedb/8f2b/25b348329f39/49cf48898cba/00475641700101-p/00475641700101-p.jpg?ts=1780323205968&w=184", 10m, "Pretty floral elastic headband",        7, 2, null, null },
                    { 123, "Kids Mini Bag",          "https://static.zara.net/assets/public/53c3/6137/77f14bfd8185/784f6a474f8e/00009549600-e1/00009549600-e1.jpg?ts=1776062396369&w=184", 18m, "Small zip-up mini bag",                 7, 1, null, null },
                    { 124, "Girls Bracelet",         "https://static.zara.net/assets/public/98ef/eab6/86524f0fbc85/3334d47ff2f9/01554512620-090-e1/01554512620-090-e1.jpg?ts=1773661662317&w=184", 12m, "Delicate beaded bracelet",              7, 2, null, null },
                    { 125, "Girls Necklace",         "https://static.zara.net/assets/public/cf55/663b/8c5149f783aa/91e3e3210726/06303533982-090-e1/06303533982-090-e1.jpg?ts=1773651400359&w=184", 14m, "Dainty chain necklace",                 7, 2, null, null },
                    { 126, "Kids Baseball Cap",      "https://static.zara.net/assets/public/fb66/8cf7/19894d87bc46/01ac678c1779/01879568600-e1/01879568600-e1.jpg?ts=1778081678953&w=184", 14m, "Classic cotton baseball cap",           7, 1, null, null },
                    { 127, "Kids Bucket Hat",        "https://static.zara.net/assets/public/d75e/8929/58aa493abe29/06b92d0c2617/04701548712-e1/04701548712-e1.jpg?ts=1775051598247&w=184", 16m, "Casual sun bucket hat",                 7, 1, null, null },
                    { 128, "Kids Canvas Backpack",   "https://static.zara.net/assets/public/c9c9/e523/64be44ec9bec/e489df2c4e18/03772584712-e1/03772584712-e1.jpg?ts=1779369525136&w=184", 28m, "Lightweight canvas school backpack",    7, 1, null, null },
                    { 129, "Kids Patterned Bag",     "https://static.zara.net/assets/public/9693/d22e/486844e09156/bb4bddddb5f9/16705730700-p/16705730700-p.jpg?ts=1776870323637&w=184", 22m, "Fun patterned tote bag",                7, 2, null, null },
                    { 130, "Girls Scrunchie Set",    "https://static.zara.net/assets/public/65d9/65b6/9fbf4c6984c3/7184becd8f0b/00475639252100-p/00475639252100-p.jpg?ts=1780481591545&w=184",  8m, "Set of elastic scrunchies",             7, 2, null, null },
                    { 131, "Kids Leather Belt",      "https://static.zara.net/assets/public/52ec/1281/61834bc0837b/305b6d2eecdd/04319505140-e1/04319505140-e1.jpg?ts=1776951753785&w=184", 14m, "Classic leather adjustable belt",       7, 1, null, null },
                    { 132, "Kids Woven Belt",        "https://static.zara.net/assets/public/ad30/8cfa/f62f4f9bb0f6/78b6aa838ee9/04319505140100-p/04319505140100-p.jpg?ts=1780411970284&w=184", 16m, "Braided woven fabric belt",             7, 1, null, null },
                    { 133, "Kids Canvas Belt",       "https://static.zara.net/assets/public/77c7/2b63/c27747b994df/2312e898542e/04319618330-e1/04319618330-e1.jpg?ts=1773763271100&w=184", 14m, "Casual canvas snap belt",               7, 1, null, null },
                    { 134, "Kids Crossbody Bag",     "https://static.zara.net/assets/public/2610/f5e6/62ac4a2ca93e/571efefc962f/07243591700400-p/07243591700400-p.jpg?ts=1780308105507&w=184", 20m, "Compact mini crossbody bag",            7, 2, null, null },
                    { 135, "Girls Charm Bracelet",   "https://static.zara.net/assets/public/d6cb/3635/51cf4c29a74c/f839b535266c/00441639600400-p/00441639600400-p.jpg?ts=1780307593315&w=184", 14m, "Sweet charm-studded bracelet",          7, 2, null, null },
                    { 136, "Kids Mini Wallet",       "https://static.zara.net/assets/public/bb66/5093/223f45da8b8b/4d5b00c1e42e/05213652400400-p/05213652400400-p.jpg?ts=1779801230585&w=184", 12m, "Small zip-around wallet",               7, 1, null, null },
                    { 137, "Kids Knit Beanie",       "https://static.zara.net/assets/public/36d0/c4d9/a50045aba187/1541dc978e62/11161730050-e1/11161730050-e1.jpg?ts=1777381764075&w=184", 16m, "Soft ribbed-knit beanie hat",           7, 1, null, null },
                    { 138, "Kids Winter Scarf",      "https://static.zara.net/assets/public/8292/f7ff/b831414ebdaf/5e21da5d4ba5/11144730500-e1/11144730500-e1.jpg?ts=1777284924505&w=184", 14m, "Cosy knitted winter scarf",             7, 1, null, null },
                    { 139, "Kids Flat Cap",          "https://static.zara.net/assets/public/1eef/c9a9/1c434d59a346/92dd1f2930cb/01296602800-e1/01296602800-e1.jpg?ts=1772613562680&w=184", 18m, "Vintage-style flat cap",                7, 1, null, null },
                    { 140, "Kids Bobble Hat",        "https://static.zara.net/assets/public/e91c/a6d9/fd4a4b54ba04/49dc60fe703f/01296650800-e1/01296650800-e1.jpg?ts=1741343790084&w=184", 14m, "Fluffy bobble-top knit hat",            7, 1, null, null },
                });

            // Insert new Sale products (IDs 141-165)
            migrationBuilder.InsertData(
                table: "KidsCards",
                columns: new[] { "Id", "Title", "ImageUrl", "Price", "Description", "KidsCategoryId", "KidsProductTypeId", "DiscountPercent", "Color" },
                values: new object[,]
                {
                    { 141, "Girls Sale Jacket",       "https://static.zara.net/assets/public/2498/f971/008448d29cfc/114336728d0f/05431330526-e1/05431330526-e1.jpg?ts=1773076445459&w=133",   45m, "Discounted girls jacket",          8, 2, 30, null },
                    { 142, "Boys Sale Hoodie",        "https://static.zara.net/assets/public/2d56/8439/d0e84b5aa7e3/c1d75d96ced2/01880761700-e1/01880761700-e1.jpg?ts=1768220947219&w=133",   38m, "Cosy boys hoodie on sale",         8, 1, 25, null },
                    { 143, "Kids Sale Set",           "https://static.zara.net/assets/public/b449/b446/f5c447a7bcd0/f2ada402ba15/00595625712-e1/00595625712-e1.jpg?ts=1773672686400&w=133",   22m, "Matching kids two-piece set",      8, 2, 40, null },
                    { 144, "Kids Sale Shorts",        "https://static.zara.net/assets/public/0aa6/a182/e1c846dfbd6b/0a348bbbc589/05992770800-p/05992770800-p.jpg?ts=1754563566779&w=133",     18m, "Light summer shorts on sale",      8, 1, 30, null },
                    { 145, "Girls Sale Trousers",     "https://static.zara.net/assets/public/f5db/8592/1aac4b39b8d7/5665f2674938/00962611104-p/00962611104-p.jpg?ts=1773749289167&w=133",     28m, "Discounted girls trousers",        8, 2, 20, null },
                    { 146, "Kids Sale Shirt",         "https://static.zara.net/assets/public/f23e/b11a/9c814316ad6f/b01334c19498/03183401105-e1/03183401105-e1.jpg?ts=1771858593530&w=133",   24m, "Smart shirt at sale price",        8, 1, 25, null },
                    { 147, "Kids Sale Jeans",         "https://static.zara.net/assets/public/e2ca/156d/16214f8ca57c/5cf3ecb54c1d/00318600712-e1/00318600712-e1.jpg?ts=1773654267658&w=133",   32m, "Classic jeans on discount",        8, 1, 20, null },
                    { 148, "Girls Sale Dress",        "https://static.zara.net/assets/public/6b19/bf7a/ec0f41b099fc/6e711b2af378/03854648661-p/03854648661-p.jpg?ts=1770146759919&w=133",     40m, "Pretty girls dress on sale",       8, 2, 35, null },
                    { 149, "Kids Sale Jumpsuit",      "https://static.zara.net/assets/public/06a3/bf69/748c4f9ba685/50b765b6c56b/03391645306100-p/03391645306100-p.jpg?ts=1775461473955&w=133", 35m, "Fun kids jumpsuit at discount",  8, 2, 30, null },
                    { 150, "Boys Sale Cardigan",      "https://static.zara.net/assets/public/b756/3c81/a167425e990a/672589f42064/05644763800-e1/05644763800-e1.jpg?ts=1765543177228&w=133",   30m, "Soft boys cardigan on sale",       8, 1, 25, null },
                    { 151, "Girls Sale Top Set",      "https://static.zara.net/assets/public/3a15/a3a7/3f454158bbe0/eaa554d58d29/03391644630100-p/03391644630100-p.jpg?ts=1775461472095&w=133", 36m, "Girls matching top set on sale", 8, 2, 40, null },
                    { 152, "Boys Sale T-Shirt",       "https://static.zara.net/assets/public/6508/187e/ca614825ac46/9f00c48f63a9/00962698500-p/00962698500-p.jpg?ts=1772197563850&w=133",     18m, "Graphic tee at sale price",        8, 1, 20, null },
                    { 153, "Girls Sale Skirt",        "https://static.zara.net/assets/public/b43d/6c38/262344b18a2f/73648d83c44d/06688604250-e1/06688604250-e1.jpg?ts=1773060298818&w=133",   22m, "Floaty girls skirt on discount",   8, 2, 30, null },
                    { 154, "Kids Sale Coat",          "https://static.zara.net/assets/public/577e/5cd4/a4334223aefe/60cae71173f4/01443679959-e1/01443679959-e1.jpg?ts=1770381721035&w=133",   55m, "Warm kids coat on sale",           8, 1, 40, null },
                    { 155, "Girls Sale Top",          "https://static.zara.net/assets/public/f577/2027/337a40ffb15d/6b38c69e4a1a/00208606620-p/00208606620-p.jpg?ts=1772795695632&w=133",     20m, "Casual girls top on discount",     8, 2, 25, null },
                    { 156, "Kids Sale Jumper",        "https://static.zara.net/assets/public/2db2/8c4e/588342d5a016/9750837fd768/03253698615-e1/03253698615-e1.jpg?ts=1775747778751&w=133",   28m, "Cosy jumper on sale",              8, 1, 30, null },
                    { 157, "Girls Sale Blouse",       "https://static.zara.net/assets/public/09fd/a518/38514ed4872f/15d2491a32be/00208606620-e1/00208606620-e1.jpg?ts=1772038440519&w=133",   22m, "Pretty blouse at discount",        8, 2, 20, null },
                    { 158, "Kids Sale Sweatshirt",    "https://static.zara.net/assets/public/7ce0/55b8/99b2435a847b/9f1112c0ea50/20310421999-000-e1/20310421999-000-e1.jpg?ts=1774456143164&w=133", 30m, "Cosy sweatshirt on sale",    8, 1, 30, null },
                    { 159, "Boys Sale Shorts",        "https://static.zara.net/assets/public/32ce/e069/0b8c449c86e3/9b2c7db28ef5/09144605807-e1/09144605807-e1.jpg?ts=1768549816264&w=133",   20m, "Boys summer shorts on sale",       8, 1, 25, null },
                    { 160, "Girls Sale Leggings",     "https://static.zara.net/assets/public/f845/09f7/6e804e5694b4/210fa7a56707/00039705500-p/00039705500-p.jpg?ts=1767875633984&w=133",     16m, "Comfy leggings on discount",       8, 2, 20, null },
                    { 161, "Kids Sale Joggers",       "https://static.zara.net/assets/public/21f2/fe62/0cab4e47a9f5/b158d3ccedf4/05537601717-p/05537601717-p.jpg?ts=1768466026689&w=133",     24m, "Jersey joggers at sale price",     8, 1, 30, null },
                    { 162, "Girls Sale Sweatpants",   "https://static.zara.net/assets/public/27cd/5220/988f44209d62/67e6867137c7/03183619712-p/03183619712-p.jpg?ts=1768466026546&w=133",     26m, "Soft girls sweatpants on sale",    8, 2, 25, null },
                    { 163, "Kids Sale Parka",         "https://static.zara.net/assets/public/3283/a735/c5ea433f97fd/930d9b184715/12131630700003-p/12131630700003-p.jpg?ts=1759751687728&w=133", 60m, "Warm hooded parka on sale",     8, 1, 35, null },
                    { 164, "Boys Sale Shirt",         "https://static.zara.net/assets/public/4bea/6991/7c05463bbf38/fc0544a34810/08054611802-p/08054611802-p.jpg?ts=1768466026653&w=133",     24m, "Smart boys shirt on discount",     8, 1, 20, null },
                    { 165, "Kids Sale Puffer Jacket", "https://static.zara.net/assets/public/0b63/102c/183f405ca40e/47defd5b9db5/08073700800-e1/08073700800-e1.jpg?ts=1752679754454&w=133",  50m, "Padded puffer jacket on sale",     8, 1, 30, null },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove new products
            migrationBuilder.DeleteData(table: "KidsCards", keyColumn: "Id", keyValues: new object[] { 84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165 });

            // Restore original Unsplash image URLs
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 23, column: "ImageUrl", value: "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 24, column: "ImageUrl", value: "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 25, column: "ImageUrl", value: "https://images.unsplash.com/photo-1550639525-c97d455acf70?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 26, column: "ImageUrl", value: "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 27, column: "ImageUrl", value: "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 28, column: "ImageUrl", value: "https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 29, column: "ImageUrl", value: "https://images.unsplash.com/photo-1597248881519-db089b3a0508?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 30, column: "ImageUrl", value: "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 31, column: "ImageUrl", value: "https://images.unsplash.com/photo-1553361371-9b22f78e8b1d?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 32, column: "ImageUrl", value: "https://images.unsplash.com/photo-1485462537746-965f33f7f6a7?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 33, column: "ImageUrl", value: "https://images.unsplash.com/photo-1499002238440-d264edd596ec?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 34, column: "ImageUrl", value: "https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 35, column: "ImageUrl", value: "https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 36, column: "ImageUrl", value: "https://images.unsplash.com/photo-1471286174890-9c112ac6f609?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 37, column: "ImageUrl", value: "https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=400&q=80");

            // Restore Sleepwear category
            migrationBuilder.InsertData(table: "KidsCategories", columns: new[] { "Id", "Name" }, values: new object[,] { { 4, "Sleepwear" } });

            // Restore Sleepwear products
            migrationBuilder.InsertData(
                table: "KidsCards",
                columns: new[] { "Id", "Title", "ImageUrl", "Price", "Description", "KidsCategoryId", "KidsProductTypeId", "DiscountPercent", "Color" },
                values: new object[,]
                {
                    { 19, "Girls Star Pyjamas", "https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=400&q=80", 22m, "Soft star-print pyjama set", 4, 2, null, null },
                    { 20, "Boys Dino Pyjamas",  "https://images.unsplash.com/photo-1586105449897-20b5efeb3233?w=400&q=80", 22m, "Fun dinosaur pyjama set",    4, 1, null, null },
                    { 21, "Baby Sleepsuit",     "https://images.unsplash.com/photo-1567578301402-c43d6dbef7de?w=400&q=80", 18m, "Warm fleece baby sleepsuit", 4, 1, null, null },
                    { 22, "Girls Nightgown",    "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400&q=80", 20m, "Cosy cotton nightgown",      4, 2, null, null },
                });
        }
    }
}
