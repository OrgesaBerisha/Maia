using System.ComponentModel.DataAnnotations;

namespace Maia.Models
{
    public class Wishlist
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<WishlistItem> WishlistItems { get; set; } = new();
    }
}