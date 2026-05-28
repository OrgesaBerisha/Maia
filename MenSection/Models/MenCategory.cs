using System.ComponentModel.DataAnnotations;

public class MenCategory
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<MenCards>? MenCards { get; set; }
}