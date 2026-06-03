using System.ComponentModel.DataAnnotations;

namespace KidsSection.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public int ProductId { get; set; }
        public KidsCards? Product { get; set; }
        public int Quantity { get; set; }
    }
}
