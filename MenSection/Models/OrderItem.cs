using System.ComponentModel.DataAnnotations;

namespace MenSection.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        public MenCards? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
