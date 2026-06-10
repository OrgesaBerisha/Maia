namespace Maia.Data.DTO
{
    public class AddToWishlistDto
    {
        public int ProductId { get; set; }
        public string Source { get; set; } = "WOMAN";
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public decimal? ProductPrice { get; set; }
    }
}