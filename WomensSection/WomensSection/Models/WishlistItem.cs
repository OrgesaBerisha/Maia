using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maia.Models
{
    public class WishlistItem
    {
        [Key]
        public int Id { get; set; }

        public int WishlistId { get; set; }
        public Wishlist Wishlist { get; set; } = null!;

        public int ProductId { get; set; }
        public string Source { get; set; } = "WOMAN";
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public decimal? ProductPrice { get; set; }

        [NotMapped]
        public CardsWomen? Product { get; set; }
    }
}