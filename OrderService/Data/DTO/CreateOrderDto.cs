namespace OrderService.Data.DTO
{
    public class CreateOrderDto
    {
        public string FullName        { get; set; } = string.Empty;
        public string Email           { get; set; } = string.Empty;
        public string Phone           { get; set; } = string.Empty;
        public string Address         { get; set; } = string.Empty;
        public string City            { get; set; } = string.Empty;
        public string PostalCode      { get; set; } = string.Empty;
        public string Country         { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string PaymentMethod   { get; set; } = "cash";
        public List<OrderItemDto>? Items { get; set; }
    }

    public class OrderItemDto
    {
        public int    ProductId     { get; set; }
        public string ProductSource { get; set; } = string.Empty;
        public string ProductName   { get; set; } = string.Empty;
        public string? ImageUrl     { get; set; }
        public decimal Price        { get; set; }
        public int    Quantity      { get; set; }
    }
}
