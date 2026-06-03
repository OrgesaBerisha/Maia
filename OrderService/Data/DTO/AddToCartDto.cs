namespace OrderService.Data.DTO
{
    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public string ProductSource { get; set; } = string.Empty; // "women", "men", "kids"
        public string ProductName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
