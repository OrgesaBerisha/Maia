using System.ComponentModel.DataAnnotations;

namespace Maia.Models
{
    public class KidsCategory
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<KidsViewAllCards>? KidsProducts { get; set; }
    }
}
