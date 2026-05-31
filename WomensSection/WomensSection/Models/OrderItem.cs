using System.ComponentModel.DataAnnotations;

namespace Maia.Models;

public class OrderItem
{
    [Key]
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }

    public int ProductId { get; set; } // CardsWomen

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}