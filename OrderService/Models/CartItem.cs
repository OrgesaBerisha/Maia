using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        public int ProductId { get; set; }
        public string ProductSource { get; set; } = string.Empty; // "women", "men", "kids"
        public string ProductName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
    }
}
