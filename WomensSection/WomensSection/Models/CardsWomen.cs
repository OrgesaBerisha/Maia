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

        public int WomanCategoryId { get; set; }

        // (IMPORTANT për HasData stability)
        public WomanCategory? WomanCategory { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}