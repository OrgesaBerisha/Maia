using System.ComponentModel.DataAnnotations;

namespace Maia.Models
{
    public class ProductReview
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;

        public int ProductId { get; set; }
        public string Source { get; set; } = "WOMAN"; // WOMAN, MAN, KIDS

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
