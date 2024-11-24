namespace AppAPI.Models.DTO
{
    public class TransactionHistoryDTO
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TotalAmount { get; set; }
    }
}
