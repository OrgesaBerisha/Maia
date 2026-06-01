using System.ComponentModel.DataAnnotations;

namespace Maia.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }  // ← SHTUAR

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<CartItem> CartItems { get; set; } = new();
    }
}