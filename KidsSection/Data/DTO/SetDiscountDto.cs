using System.ComponentModel.DataAnnotations;

namespace KidsSection.Data.DTO
{
    public class SetDiscountDto
    {
        [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100")]
        public int? DiscountPercent { get; set; }
    }
}
