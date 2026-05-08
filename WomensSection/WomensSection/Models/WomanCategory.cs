using System.ComponentModel.DataAnnotations;

namespace Maia.Models
{
    public class WomanCategory
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        // Dresses, Shoes, Jackets, Bags
    }
}

