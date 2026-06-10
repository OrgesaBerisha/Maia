using MenSection.Models;
using System.ComponentModel.DataAnnotations;

public class MenCards
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public int MenCategoryId { get; set; }
    public MenCategory? MenCategory { get; set; }


    public string Description { get; set; } = string.Empty;

    public string? Color { get; set; }
    public int? DiscountPercent { get; set; }
    public int? OriginalCategoryId { get; set; }
}