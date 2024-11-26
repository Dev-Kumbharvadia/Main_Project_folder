namespace AppAPI.Models.DTOs
{
    public class SalesBySellerDTO
    {
        public Guid SellerId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public double TotalAmountSold { get; set; }
        public int TotalProductsSold { get; set; }
        public List<ItemSoldDTO> ItemsSold { get; set; } // List of items sold by the seller
    }
}
