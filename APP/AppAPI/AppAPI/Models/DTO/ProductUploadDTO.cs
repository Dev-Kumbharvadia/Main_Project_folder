namespace AppAPI.Models.DTO
{
    public class ProductUploadDTO
    {
        public string ProductName { get; set; } = null!; // Required
        public string? Description { get; set; } // Optional
        public IFormFile ImageFile { get; set; } = null!; // Required for the image
        public double Price { get; set; } // Required
        public int StockQuantity { get; set; } // Default 0
        public Guid SellerId { get; set; } // Required (Foreign Key to Users)
    }
}
