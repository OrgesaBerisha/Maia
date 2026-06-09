using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace KidsSection.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKidsImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Baby (category 1)
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 1,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/8c/04/8c04c5efaa10c9ce45aa90a0736a3f655380e82d.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 5,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/42/f4/42f4c83ce439dacd1f7139ef729670df582b0fe0.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 9,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/c9/e0/c9e006d06dccb847daaac634ec3acac6c17807f5.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 10, column: "ImageUrl", value: "https://image.hm.com/assets/hm/d8/1e/d81e720a2bb94a1f788417943316de657e36d28a.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 11, column: "ImageUrl", value: "https://image.hm.com/assets/hm/1b/68/1b68a32d8c0c25a3570a41bf5669d2924d36ed4c.jpg?imwidth=1260");

            // Girls (category 2)
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 2,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/ab/15/ab15b8cd3b78ac8ba76768f9223a104734884e52.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 6,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/de/a1/dea1c170d095bae1a29b0606b6c8574890f87d3e.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 8,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/4a/f1/4af1c886bf01af58765b5f9a23fc5b704b92460a.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 12, column: "ImageUrl", value: "https://image.hm.com/assets/hm/9f/0e/9f0e60edfbc8d7676ff7c7d1efd9c91505c7644a.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 13, column: "ImageUrl", value: "https://image.hm.com/assets/hm/e6/10/e61010dcc4134f59cf924bd16d37057ca1341391.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 14, column: "ImageUrl", value: "https://image.hm.com/assets/hm/c6/38/c638eb92d0419205d9c79c2bfc03afa13785b5d0.jpg?imwidth=1260");

            // Boys (category 3)
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 3,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/4b/19/4b1928a639cd4e4661d4ecc41c0a1e03e27730e7.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 4,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/41/be/41bed8a23b34c5083aa8a0bbbc9755592c6f716b.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 7,  column: "ImageUrl", value: "https://image.hm.com/assets/hm/43/a3/43a3a8d8a832d2d8dd1ad9391904345c387ca380.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 15, column: "ImageUrl", value: "https://image.hm.com/assets/hm/b6/6e/b66eee7aa009d6f0a0e72b112bc4f99ea6af8d05.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 16, column: "ImageUrl", value: "https://image.hm.com/assets/hm/fa/68/fa6891f65019c07a31f6fe616ccf8e5e53dc26b0.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 17, column: "ImageUrl", value: "https://image.hm.com/assets/hm/30/31/3031eb8ab1421f329731d54b8b35b15f1c8b6711.jpg?imwidth=1260");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 18, column: "ImageUrl", value: "https://image.hm.com/assets/hm/0d/2a/0d2a9d03b5aca7b15f680726b3186520bd1513b6.jpg?imwidth=1260");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Baby
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 1,  column: "ImageUrl", value: "https://images.unsplash.com/photo-1522771930-78848d9293e8?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 5,  column: "ImageUrl", value: "https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 9,  column: "ImageUrl", value: "https://images.unsplash.com/photo-1607227893600-9bc90f1ead82?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 10, column: "ImageUrl", value: "https://images.unsplash.com/photo-1574179618836-b7b81b2e1a4d?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 11, column: "ImageUrl", value: "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b?w=400&q=80");
            // Girls
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 2,  column: "ImageUrl", value: "https://images.unsplash.com/photo-1508766917616-d22f3f1eea14?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 6,  column: "ImageUrl", value: "https://images.unsplash.com/photo-1518831959646-742c3a14ebf7?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 8,  column: "ImageUrl", value: "https://images.unsplash.com/photo-1543087903-1ac2ec7aa8c5?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 12, column: "ImageUrl", value: "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 13, column: "ImageUrl", value: "https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 14, column: "ImageUrl", value: "https://images.unsplash.com/photo-1566174053879-31528523f8ae?w=400&q=80");
            // Boys
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 3,  column: "ImageUrl", value: "https://images.unsplash.com/photo-1503454537195-1dcabb73ffb9?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 4,  column: "ImageUrl", value: "https://images.unsplash.com/photo-1471286174890-9c112ac6f609?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 7,  column: "ImageUrl", value: "https://images.unsplash.com/photo-1591195853828-11db59a44f43?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 15, column: "ImageUrl", value: "https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 16, column: "ImageUrl", value: "https://images.unsplash.com/photo-1556821840-3a63f15732ce?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 17, column: "ImageUrl", value: "https://images.unsplash.com/photo-1583744946564-b52ac1c389c8?w=400&q=80");
            migrationBuilder.UpdateData(table: "KidsCards", keyColumn: "Id", keyValue: 18, column: "ImageUrl", value: "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400&q=80");
        }
    }
}
