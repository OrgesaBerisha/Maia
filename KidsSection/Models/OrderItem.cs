using System.ComponentModel.DataAnnotations;

namespace KidsSection.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        public KidsCards? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
