using System.ComponentModel.DataAnnotations;

namespace KidsSection.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
