namespace AppAPI.Models.DTOs
{
    public class ItemSoldDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public Double Price { get; set; } // Not Null
        public int TotalQuantitySold { get; set; }
        public double TotalAmountSold { get; set; }
    }
}
