namespace Maia.Data.DTO
{
    public class CreateCardsWomenDto
    {
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;

        public int WomanCategoryId { get; set; } // 👈 KJO DUHET
    }
}