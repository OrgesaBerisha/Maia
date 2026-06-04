namespace MenSection.Data.DTO
{
    public class CreateMenCardsDto
    {
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int MenCategoryId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
