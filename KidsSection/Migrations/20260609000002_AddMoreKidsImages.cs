using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace KidsSection.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreKidsImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "KidsCards",
                columns: new[] { "Id", "Title", "ImageUrl", "Price", "Description", "KidsCategoryId", "KidsProductTypeId", "DiscountPercent", "Color" },
                values: new object[,]
                {
                    // Baby (extra — images 6-22)
                    { 38, "Baby Wrap Cardigan",       "https://image.hm.com/assets/hm/29/71/2971d502c2d645aeedf50f3bf379b61c6145ba31.jpg?imwidth=1260", 18m,  "Soft knitted wrap cardigan",        1, 2, null, null },
                    { 39, "Baby Cotton Playsuit",     "https://image.hm.com/assets/hm/46/0a/460a8b7dacb6f4da9c197f578ee68fa9a7bb3889.jpg?imwidth=1260", 16m,  "Comfortable cotton playsuit",       1, 1, null, null },
                    { 40, "Baby Ribbed Set",          "https://image.hm.com/assets/hm/70/f1/70f1d834f8fdb12436f90818d681717c8b8545c6.jpg?imwidth=1260", 20m,  "Stretchy ribbed two-piece set",     1, 2, null, null },
                    { 41, "Baby Printed Romper",      "https://image.hm.com/assets/hm/47/6e/476e7c1f6327291192eb8896449d64f9ed0d507b.jpg?imwidth=1260", 14m,  "Cute printed all-in-one romper",    1, 2, null, null },
                    { 42, "Baby Velour Suit",         "https://image.hm.com/assets/hm/c3/bc/c3bc47a6e0c2465dd9c8814d86cff807eb51d535.jpg?imwidth=1260", 22m,  "Soft velour top and trouser set",   1, 1, null, null },
                    { 43, "Baby Patterned Top",       "https://image.hm.com/assets/hm/a0/c2/a0c2086db06ee77e222a7b1b51dabc32e5ad8d4e.jpg?imwidth=1260", 12m,  "Colourful patterned long-sleeve",   1, 2, null, null },
                    { 44, "Baby Wrap Body",           "https://image.hm.com/assets/hm/f5/59/f5590708a7b9210721916490df86090944f894dd.jpg?imwidth=1260", 10m,  "Easy wrap-fastening body",          1, 1, null, null },
                    { 45, "Baby Quilted Jacket",      "https://image.hm.com/assets/hm/52/71/5271451836c9911950b9159ac4ec2454b9387dc3.jpg?imwidth=1260", 28m,  "Warm quilted zip-up jacket",        1, 1, null, null },
                    { 46, "Baby Ruffle Dress",        "https://image.hm.com/assets/hm/27/e6/27e635b673c9238ebb5567602e00b8e99d062253.jpg?imwidth=1260", 20m,  "Sweet ruffle-trim baby dress",      1, 2, null, null },
                    { 47, "Baby Hooded Sweatshirt",   "https://image.hm.com/assets/hm/f5/c4/f5c4093647b279f2e047bc80d1c03906067587cc.jpg?imwidth=1260", 22m,  "Cosy hooded baby sweatshirt",       1, 1, null, null },
                    { 48, "Baby Jersey Leggings",     "https://image.hm.com/assets/hm/d8/65/d8651d1d12ff9a02d7d7034f05ed8d06ed50d83a.jpg?imwidth=1260", 12m,  "Soft stretch jersey leggings",      1, 2, null, null },
                    { 49, "Baby Smocked Dress",       "https://image.hm.com/assets/hm/7b/33/7b33d1353452a261ea6ef4f1c0fa214e2b4cb0b7.jpg?imwidth=1260", 24m,  "Delicate smocked cotton dress",     1, 2, null, null },
                    { 50, "Baby Terry Joggers",       "https://image.hm.com/assets/hm/60/87/60870dadfcc29e8fb83d60bd0da326c649e677e8.jpg?imwidth=1260", 16m,  "Soft terry-cloth jogger pants",     1, 1, null, null },
                    { 51, "Baby Printed Leggings",    "https://image.hm.com/assets/hm/00/9b/009badc880eac55dccf0a48bd0fb8f48642564aa.jpg?imwidth=1260", 12m,  "Fun printed stretch leggings",      1, 2, null, null },
                    { 52, "Baby Long-Sleeve Top",     "https://image.hm.com/assets/hm/a1/82/a182beecb2b7ae190ed14c3f7b2a1f3d41b3a648.jpg?imwidth=1260", 14m,  "Soft long-sleeve jersey top",       1, 1, null, null },
                    { 53, "Baby Zip Sleepsuit",       "https://image.hm.com/assets/hm/cb/fd/cbfdd42d2ae250b84240b35ed1cec21f652c79b9.jpg?imwidth=1260", 18m,  "Cosy zip-fastening sleepsuit",      1, 1, null, null },
                    { 54, "Baby Denim Set",           "https://image.hm.com/assets/hm/9f/50/9f50289fb4dfd9f089101165011843d712927cd0.jpg?imwidth=1260", 24m,  "Cute denim jacket and trouser set", 1, 1, null, null },
                    // Girls (extra — images 7-21)
                    { 55, "Girls Embroidered Blouse", "https://image.hm.com/assets/hm/98/65/98652f9415cabc5ff3c0f3fa1da8363f268bddf3.jpg?imwidth=1260", 24m,  "Floral embroidered cotton blouse",  2, 2, null, null },
                    { 56, "Girls Pleated Skirt",      "https://image.hm.com/assets/hm/f6/a4/f6a4628ff4e95b72d299ca89fca1a32630b6e60e.jpg?imwidth=1260", 22m,  "Pretty pleated midi skirt",         2, 2, null, null },
                    { 57, "Girls Ruffle Top",         "https://image.hm.com/assets/hm/54/80/548024730d8318c5e6f7825019b4fabb5c863946.jpg?imwidth=1260", 20m,  "Sweet ruffle-trim woven top",       2, 2, null, null },
                    { 58, "Girls Floral Jumpsuit",    "https://image.hm.com/assets/hm/d2/10/d2105e0304ea90b6889809976e0dc7f1f5aafb96.jpg?imwidth=1260", 38m,  "Breezy floral wide-leg jumpsuit",   2, 2, null, null },
                    { 59, "Girls Striped T-Shirt",    "https://image.hm.com/assets/hm/4c/4b/4c4bf3d062319d0cb0105bbe07429279b4e18e47.jpg?imwidth=1260", 18m,  "Classic striped cotton tee",        2, 2, null, null },
                    { 60, "Girls Velvet Dress",       "https://image.hm.com/assets/hm/75/28/7528ca97387e613f693fd30cf8eca320ccd18249.jpg?imwidth=1260", 42m,  "Elegant velvet party dress",        2, 2, null, null },
                    { 61, "Girls Tie-Dye Hoodie",     "https://image.hm.com/assets/hm/14/d1/14d16be739a74579d3d444d582207291de231fab.jpg?imwidth=1260", 28m,  "Trendy tie-dye zip hoodie",         2, 2, null, null },
                    { 62, "Girls Printed Joggers",    "https://image.hm.com/assets/hm/70/10/701055386810b6eacef999ffd08091505074a7cb.jpg?imwidth=1260", 22m,  "Soft printed jersey joggers",       2, 2, null, null },
                    { 63, "Girls Denim Jacket",       "https://image.hm.com/assets/hm/27/c8/27c8197972224ca42f051bbc057e172d92ea5d18.jpg?imwidth=1260", 38m,  "Classic stone-washed denim jacket", 2, 2, null, null },
                    { 64, "Girls Knit Cardigan",      "https://image.hm.com/assets/hm/14/bc/14bc0bfc778be9d28651e5c1cf0c50dc57190328.jpg?imwidth=1260", 32m,  "Cosy open-front knit cardigan",     2, 2, null, null },
                    { 65, "Girls Sequin Top",         "https://image.hm.com/assets/hm/02/94/029443545c4bc9e858c02d0464bb83979ebd2627.jpg?imwidth=1260", 26m,  "Sparkly sequin party top",          2, 2, null, null },
                    { 66, "Girls Wide-Leg Trousers",  "https://image.hm.com/assets/hm/99/c4/99c41f96ad10243a77e929616819991282af2cf3.jpg?imwidth=1260", 28m,  "Relaxed wide-leg woven trousers",   2, 2, null, null },
                    { 67, "Girls Linen Shorts",       "https://image.hm.com/assets/hm/70/c6/70c6e76b648d167c01cc18287bea90368de6c96f.jpg?imwidth=1260", 18m,  "Lightweight linen pull-on shorts",  2, 2, null, null },
                    { 68, "Girls Smocked Dress",      "https://image.hm.com/assets/hm/7e/fa/7efabdd51642bc55f269013d409814214cc186c8.jpg?imwidth=1260", 36m,  "Pretty smocked cotton dress",       2, 2, null, null },
                    { 69, "Girls Cargo Pants",        "https://image.hm.com/assets/hm/13/79/13791492d295b4dbe4914c53751da8524097dd4d.jpg?imwidth=1260", 28m,  "Practical multi-pocket cargo pants",2, 2, null, null },
                    // Boys (extra — images 8-21)
                    { 70, "Boys Track Jacket",        "https://image.hm.com/assets/hm/3a/c2/3ac259f71ba8aa0f00f24b485f7b403867906d46.jpg?imwidth=1260", 36m,  "Sporty zip-up track jacket",        3, 1, null, null },
                    { 71, "Boys Printed Sweatshirt",  "https://image.hm.com/assets/hm/48/52/485224f72c37e68cc038ab6258f17497c9f60df9.jpg",              28m,  "Cool graphic print sweatshirt",     3, 1, null, null },
                    { 72, "Boys Fleece Hoodie",       "https://image.hm.com/assets/hm/26/13/26131d5d278c6ae384f39a28f82751f80cfe2398.jpg",              32m,  "Warm soft-fleece pullover hoodie",  3, 1, null, null },
                    { 73, "Boys Slim Fit Jeans",      "https://image.hm.com/assets/hm/07/d1/07d1bd3ba0b07a100e305145a11366b902af4877.jpg?imwidth=1260", 30m,  "Modern slim-fit denim jeans",       3, 1, null, null },
                    { 74, "Boys Tech Joggers",        "https://image.hm.com/assets/hm/af/fa/affa1f4d54b02148f78885a480ecf134c48358d5.jpg?imwidth=1260", 26m,  "Lightweight tech-fabric joggers",   3, 1, null, null },
                    { 75, "Boys Linen Shirt",         "https://image.hm.com/assets/hm/89/6d/896d4847603dda4310814f210d29a80850550287.jpg?imwidth=1260", 22m,  "Breezy linen short-sleeve shirt",   3, 1, null, null },
                    { 76, "Boys Board Shorts",        "https://image.hm.com/assets/hm/4c/43/4c43460eaad395f95d5355014f708b84f363fc20.jpg?imwidth=1260", 20m,  "Quick-dry patterned board shorts",  3, 1, null, null },
                    { 77, "Boys Zip Fleece",          "https://image.hm.com/assets/hm/ee/c9/eec90fc1e30eca86b6d5e5ceccc0dacba7d7e7fc.jpg?imwidth=1260", 34m,  "Cosy full-zip fleece top",          3, 1, null, null },
                    { 78, "Boys Striped Tee",         "https://image.hm.com/assets/hm/91/8e/918ea167ca1684dffa6226d7155201f9ad496ad8.jpg?imwidth=1260", 16m,  "Classic navy striped jersey tee",   3, 1, null, null },
                    { 79, "Boys Utility Shorts",      "https://image.hm.com/assets/hm/6b/fb/6bfb18d84e07d78ddbe585acab20f1cded8cc239.jpg?imwidth=1260", 22m,  "Cargo-style utility shorts",        3, 1, null, null },
                    { 80, "Boys Quilted Jacket",      "https://image.hm.com/assets/hm/aa/b7/aab746f40e5f92895ddb564434bfca4e7d4beb2e.jpg?imwidth=1260", 44m,  "Padded quilted zip jacket",         3, 1, null, null },
                    { 81, "Boys Long-Sleeve Tee",     "https://image.hm.com/assets/hm/ff/73/ff73fe3b24d6b102f95fb3b26c03082e4c33d4a5.jpg?imwidth=1260", 18m,  "Soft jersey long-sleeve tee",       3, 1, null, null },
                    { 82, "Boys Sweatpants",          "https://image.hm.com/assets/hm/33/62/3362c401e242b81519e76f47bd0792deba934f25.jpg?imwidth=1260", 28m,  "Comfortable elastic-waist joggers", 3, 1, null, null },
                    { 83, "Boys Printed Tee",         "https://image.hm.com/assets/hm/9c/f9/9cf926b0539bb6de80b13b9e22650fdacccf338b.jpg?imwidth=1260", 18m,  "Fun graphic printed t-shirt",       3, 1, null, null },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KidsCards",
                keyColumn: "Id",
                keyValues: new object[] { 38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83 });
        }
    }
}
