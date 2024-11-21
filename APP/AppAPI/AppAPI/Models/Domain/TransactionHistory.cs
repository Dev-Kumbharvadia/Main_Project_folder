using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppAPI.Models.Domain
{
    public class TransactionHistory
    {
        [Key]
        [Required]
        public Guid TransactionId { get; set; } // Primary Key

        [Required]
        public Guid ProductId { get; set; } // Foreign Key to Products

        [Required]
        public Guid BuyerId { get; set; } // Foreign Key to Users

        [Required]
        public int Quantity { get; set; } // Not Null

        [Required]
        public double TotalAmount { get; set; } // Not Null

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow; // Default value
    }
}
