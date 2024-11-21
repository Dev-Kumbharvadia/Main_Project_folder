using System.ComponentModel.DataAnnotations;

namespace AppAPI.Models.DTO
{
    public class PurchaseDTO
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid BuyerId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double TotalAmount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    }
}
