    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System.ComponentModel.DataAnnotations;

    namespace AppAPI.Models.Domain
    {
        public class Product
        {
            public Guid ProductId { get; set; } // Primary Key

            [Required]
            public string ProductName { get; set; } = null!; // Not Null

            public string? Description { get; set; } // Optional

            public byte[] ImageContent { get; set; } = Array.Empty<byte>();

            public Double Price { get; set; } // Not Null

            public int StockQuantity { get; set; } // Default 0

            public Guid SellerId { get; set; } // Foreign Key to Users

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default value

            public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

            public User Seller { get; set; }
        }
    }
