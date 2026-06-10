using System.ComponentModel.DataAnnotations;

namespace Maia.Data.DTO
{
    public class AddReviewDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string Source { get; set; } = "WOMAN";

        [Required, Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;
    }
}
