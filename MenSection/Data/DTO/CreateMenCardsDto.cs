namespace MenSection.Data.DTO
{
    public class CreateMenCardsDto
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile Image { get; set; }
        public decimal Price { get; set; }
        public int MenCategoryId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
