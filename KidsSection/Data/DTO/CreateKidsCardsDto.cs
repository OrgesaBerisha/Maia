namespace KidsSection.Data.DTO
{
    public class CreateKidsCardsDto
    {
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int KidsCategoryId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
