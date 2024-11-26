namespace AppAPI.Models.DTOs
{
    public class SalesForAllSellersDTO
    {
        public Guid SellerId { get; set; }
        public double TotalAmountSold { get; set; }
        public int TotalProductsSold { get; set; }
        public List<ItemSoldDTO> ItemsSold { get; set; } // List of items sold by each seller
    }
}
