using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using water.Data;

namespace water.Models
{
    public class WaterDeliveryInfo
    {
        [Key]
        public int Id { get; set; }
        public string Product_Id { get; set; } = null!;
        public string Product_Name { get; set; } = null!;
        [Required]
        public DateOnly DeliveryDate { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int RemainingQuantity { get; set; }
        public string Sheet_Id { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ValidateNever]
        public string UserId { get; set; } = null!;
        [ValidateNever]
        public ApplicationUser User { get; set; } = null!;
    }
}
