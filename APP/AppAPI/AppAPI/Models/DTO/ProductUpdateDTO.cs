using System.ComponentModel.DataAnnotations;

namespace AppAPI.Models.DTO
{
    public class ProductUpdateDTO
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public string? Description { get; set; }

        public IFormFile File { get; set; }
    }
}
