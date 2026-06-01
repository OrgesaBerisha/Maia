using System.ComponentModel.DataAnnotations;

namespace Maia.Models
{
    public class WishlistItem
    {
        [Key]
        public int Id { get; set; }

        public int WishlistId { get; set; }
        public Wishlist Wishlist { get; set; } = null!;

        public int ProductId { get; set; }
        public CardsWomen Product { get; set; } = null!;  // ← SHTUAR navigation
    }
}