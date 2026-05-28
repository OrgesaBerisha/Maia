using System.ComponentModel.DataAnnotations;

public class KidsCategory
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<KidsCards>? KidsCards { get; set; }
}