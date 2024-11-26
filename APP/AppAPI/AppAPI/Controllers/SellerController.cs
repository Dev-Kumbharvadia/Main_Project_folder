using AppAPI.Data;
using AppAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly SieveProcessor _sieveProcessor;

        public SellerController(ApplicationDbContext context, IWebHostEnvironment environment, SieveProcessor sieveProcessor)
        {
            _context = context;
            _environment = environment;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet("AllSalesData")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetSalesForAllSellers()
        {
            // Fetch sales data for all sellers including details for each item sold
            var salesData = await _context.TransactionHistories
                .Include(th => th.Product) // Include Product data in TransactionHistory
                .GroupBy(th => th.Product.SellerId) // Group by SellerId
                .Select(group => new SalesForAllSellersDTO
                {
                    SellerId = group.Key,
                    TotalAmountSold = group.Sum(th => th.TotalAmount), // Sum of TotalAmount
                    TotalProductsSold = group.Sum(th => th.Quantity), // Sum of Quantity
                    ItemsSold = group
                        .GroupBy(th => new { th.ProductId, th.Product.ProductName })
                        .Select(innerGroup => new ItemSoldDTO
                        {
                            ProductId = innerGroup.Key.ProductId,
                            ProductName = innerGroup.Key.ProductName,
                            TotalQuantitySold = innerGroup.Sum(th => th.Quantity), // Total quantity sold for this product
                            TotalAmountSold = innerGroup.Sum(th => th.TotalAmount) // Total amount sold for this product
                        })
                        .ToList()
                })
                .ToListAsync();

            // If no data found, return 404 Not Found
            if (!salesData.Any())
                return NotFound(new { message = "No sales data found." });

            // Return the sales data with a success message
            return Ok(new
            {
                message = "Sales data retrieved successfully",
                data = salesData
            });
        }

        [HttpGet("SalesDataByID")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> GetSalesBySeller(Guid sellerId)
        {
            // Fetch sales data for a specific seller including details for each item sold, with price information
            var salesData = await _context.Users
                .Where(u => u.UserId == sellerId) // Filter by seller ID
                .Select(user => new SalesBySellerDTO
                {
                    SellerId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,

                    // Calculate TotalAmountSold (total money from all products sold)
                    TotalAmountSold = user.Products
                        .SelectMany(p => p.Transactions) // Join Products and TransactionHistories
                        .Sum(th => (double?)th.TotalAmount) ?? 0, // Handle null values

                    // Calculate TotalProductsSold (total number of products sold)
                    TotalProductsSold = user.Products
                        .SelectMany(p => p.Transactions) // Join Products and TransactionHistories
                        .Sum(th => (int?)th.Quantity) ?? 0, // Handle null values

                    // Get the list of items sold, with the quantity and money earned per item
                    ItemsSold = user.Products
                        .SelectMany(p => p.Transactions) // Flatten transactions for each product
                        .GroupBy(th => new { th.ProductId, th.Product.ProductName, th.Product.Price })
                        .Select(group => new ItemSoldDTO
                        {
                            ProductId = group.Key.ProductId,
                            ProductName = group.Key.ProductName,
                            Price = group.Key.Price, // Include the price of the product
                            TotalQuantitySold = group.Sum(th => th.Quantity), // Total quantity sold for this product
                            TotalAmountSold = group.Sum(th => th.Quantity * group.Key.Price) // Total amount sold for this product (quantity * price)
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            // If no data found, return 404 Not Found
            if (salesData == null)
                return NotFound(new { message = "No sales data found for the given seller." });

            // Return the sales data with a success message
            return Ok(new
            {
                message = "Sales data retrieved successfully",
                data = salesData
            });
        }


    }
}
