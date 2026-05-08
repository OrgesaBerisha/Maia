using System.ComponentModel.DataAnnotations;

namespace Maia.Models
{
    public class CardsWomen
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }

        // 👇 FK (lidhje me tabelën WomanCategory)
        public int WomanCategoryId { get; set; }
        public WomanCategory WomanCategory { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
