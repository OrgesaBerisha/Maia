namespace KidsSection.Data.DTO
{
    public class CreateKidsCardsDto
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile Image { get; set; }
        public decimal Price { get; set; }
        public int KidsProductTypeId { get; set; }
        public int KidsCategoryId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? DiscountPercent { get; set; }
    }
}
