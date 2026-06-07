using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status      { get; set; } = "Pending";
        public string FullName    { get; set; } = string.Empty;
        public string Email       { get; set; } = string.Empty;
        public string Phone       { get; set; } = string.Empty;
        public string Address     { get; set; } = string.Empty;
        public string City        { get; set; } = string.Empty;
        public string PostalCode  { get; set; } = string.Empty;
        public string Country     { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
