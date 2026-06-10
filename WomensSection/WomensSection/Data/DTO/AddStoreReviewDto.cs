using System.ComponentModel.DataAnnotations;

namespace Maia.Data.DTO
{
    public class AddStoreReviewDto
    {
        [Required, Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;
    }
}
