using System.ComponentModel.DataAnnotations;

namespace KidsSection.Models
{
    public class KidsProductType
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<KidsCards>? KidsCards { get; set; }
    }
}