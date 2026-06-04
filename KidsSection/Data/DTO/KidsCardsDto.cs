namespace KidsSection.Data.DTO
{
    public class KidsCardsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int KidsCategoryId { get; set; }
        public string? KidsCategoryName { get; set; }
        public int KidsProductTypeId { get; set; }
        public string? KidsProductTypeName { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? DiscountPercent { get; set; }
    }
}
