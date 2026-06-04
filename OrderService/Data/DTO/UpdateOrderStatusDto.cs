using System.ComponentModel.DataAnnotations;

namespace OrderService.Data.DTO
{
    public class UpdateOrderStatusDto
    {
        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Pending|Processing|Shipped|Delivered|Cancelled)$",
            ErrorMessage = "Status must be: Pending, Processing, Shipped, Delivered, or Cancelled")]
        public string Status { get; set; } = string.Empty;
    }
}
