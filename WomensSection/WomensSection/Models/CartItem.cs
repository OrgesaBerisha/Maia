using System.ComponentModel.DataAnnotations;

namespace Maia.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}