using System.ComponentModel.DataAnnotations;

namespace Maia.Data.DTO
{
    public class CreateCardsWomenDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title max length is 100")]
        public string Title { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.1, 10000, ErrorMessage = "Price must be between 0.1 and 10000")]
        public decimal Price { get; set; }

        [MaxLength(500, ErrorMessage = "Description max length is 500")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "WomanCategoryId is required")]
        public int WomanCategoryId { get; set; }
    }
}