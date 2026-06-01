using System.ComponentModel.DataAnnotations;

namespace Maia.Models
{
    public class Wishlist
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }  // ← SHTUAR

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<WishlistItem> WishlistItems { get; set; } = new();
    }
}