using System.ComponentModel.DataAnnotations;

namespace Maia.Models
{
    public class KidsViewAllCards
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public int KidsCategoryId { get; set; }

        public KidsCategory? KidsCategory { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
