using KidsSection.Models;
using System.ComponentModel.DataAnnotations;

public class KidsCards
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public int KidsCategoryId { get; set; }
    public KidsCategory? KidsCategory { get; set; }

    public int KidsProductTypeId { get; set; }
    public KidsProductType? KidsProductType { get; set; }

    public string Description { get; set; } = string.Empty;

    public int? DiscountPercent { get; set; }
}