using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<CartItem> CartItems { get; set; } = new();
    }
}
