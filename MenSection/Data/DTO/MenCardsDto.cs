namespace MenSection.Data.DTO
{
    public class MenCardsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int MenCategoryId { get; set; }
        public string? MenCategoryName { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Color { get; set; }
        public int? DiscountPercent { get; set; }
    }
}
