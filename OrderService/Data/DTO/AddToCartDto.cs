using System.ComponentModel.DataAnnotations;

namespace OrderService.Data.DTO
{
    public class AddToCartDto
    {
        [Required(ErrorMessage = "Product ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be a positive number")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product source is required")]
        [RegularExpression("^(women|men|kids)$", ErrorMessage = "Product source must be 'women', 'men', or 'kids'")]
        public string ProductSource { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Product name cannot exceed 200 characters")]
        public string ProductName { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 99999.99, ErrorMessage = "Price must be between 0.01 and 99999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }
    }
}
