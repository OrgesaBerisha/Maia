using System.ComponentModel.DataAnnotations;

namespace MenSection.Data.DTO
{
    public class CreateMenCardsDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Url(ErrorMessage = "Invalid image URL format")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 99999.99, ErrorMessage = "Price must be between 0.01 and 99999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive number")]
        public int MenCategoryId { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }
}
