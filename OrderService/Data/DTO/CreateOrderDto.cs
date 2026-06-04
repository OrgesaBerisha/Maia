using System.ComponentModel.DataAnnotations;

namespace OrderService.Data.DTO
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Shipping address is required")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Shipping address must be between 5 and 300 characters")]
        public string ShippingAddress { get; set; } = string.Empty;
    }
}
