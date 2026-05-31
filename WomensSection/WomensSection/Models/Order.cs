using System.ComponentModel.DataAnnotations;

namespace Maia.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public decimal TotalPrice { get; set; }

    //  relation
    public List<OrderItem> OrderItems { get; set; } = new();
}