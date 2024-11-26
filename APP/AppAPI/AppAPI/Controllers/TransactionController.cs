using AppAPI.Data;
using AppAPI.Models.Domain;
using AppAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        //private readonly IWebHostEnvironment _environment;
        //private readonly SieveProcessor _sieveProcessor;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
            //_environment = environment;
            //_sieveProcessor = sieveProcessor;
        }

        [HttpGet("getAllTransaction")]
        [Authorize]
        public async Task<IActionResult> getAllTransaction()
        {
            return Ok();
        }

        [HttpGet("getAllTransactionById")]
        [Authorize]
        public async Task<IActionResult> getTransactionById(Guid Id)
        {
            return Ok();
        }

        [HttpPost("MakePurchase")]
        public async Task<IActionResult> MakePurchase(PurchaseDTO purchase)
        {
            // Step 1: Validate the input
            if (purchase == null)
                return BadRequest("Purchase details are required.");

            if (purchase.Quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            if (purchase.ProductId == Guid.Empty || purchase.BuyerId == Guid.Empty)
                return BadRequest("Invalid Product or Buyer ID.");

            // Step 2: Fetch the product
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == purchase.ProductId);

            if (product == null)
                return NotFound("Product not found.");

            // Step 3: Check stock availability
            if (product.StockQuantity < purchase.Quantity)
                return BadRequest("Not enough stock available.");

            // Step 4: Calculate the total amount
            double totalAmount = product.Price * purchase.Quantity;

            // Step 5: Verify if the buyer exists
            var buyer = await _context.Users.FirstOrDefaultAsync(u => u.UserId == purchase.BuyerId);
            if (buyer == null)
                return NotFound("Buyer not found.");

            // Step 6: Create the transaction history
            var transactionHistory = new TransactionHistory
            {
                TransactionId = Guid.NewGuid(),
                ProductId = purchase.ProductId,
                BuyerId = purchase.BuyerId,
                SellerId = product.SellerId, // Set the seller from the product
                Quantity = purchase.Quantity,
                TotalAmount = totalAmount,
                TransactionDate = DateTime.UtcNow,
                Product = product, // Navigation property
                Buyer = buyer,     // Navigation property
                Seller = await _context.Users.FirstOrDefaultAsync(u => u.UserId == product.SellerId) ?? throw new InvalidOperationException("Seller not found.")
            };

            // Step 7: Update the product stock
            product.StockQuantity -= purchase.Quantity;

            // Step 8: Save changes to the database
            using var transaction = await _context.Database.BeginTransactionAsync(); // Ensure atomicity
            try
            {
                _context.TransactionHistories.Add(transactionHistory);
                _context.Products.Update(product);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred while processing the purchase: {ex.Message}");
            }

            // Step 9: Return a success response with transaction details
            return Ok(new
            {
                Message = "Purchase successful.",
                Transaction = new
                {
                    transactionHistory.TransactionId,
                    transactionHistory.TransactionDate,
                    Product = new { product.ProductId, product.ProductName, product.Price },
                    Buyer = new { buyer.UserId, buyer.Username },
                    Seller = new { transactionHistory.Seller.UserId, transactionHistory.Seller.Username },
                    transactionHistory.Quantity,
                    transactionHistory.TotalAmount
                }
            });
        }



        [HttpGet("getTransactionHistory")]
        [Authorize(Roles = "seller,admin")]
        public async Task<IActionResult> GetTransactionHistory()
        {
            // Step 1: Get the transaction history
            var transactionHistory = await _context.TransactionHistories
                .Select(t => new
                {
                    t.TransactionId,
                    t.ProductId,
                    t.Quantity,
                    t.TransactionDate,
                    t.TotalAmount
                })
                .ToListAsync();

            // Step 2: Get the product details using ProductId
            var transactionHistoryWithProductDetails = new List<TransactionHistoryDTO>();

            foreach (var transaction in transactionHistory)
            {
                var product = await _context.Products
                    .Where(p => p.ProductId == transaction.ProductId)
                    .FirstOrDefaultAsync();

                if (product != null)
                {
                    transactionHistoryWithProductDetails.Add(new TransactionHistoryDTO
                    {
                        ProductName = product.ProductName,  // Get the ProductName from the product entity
                        Quantity = transaction.Quantity,
                        TransactionDate = transaction.TransactionDate,
                        TotalAmount = transaction.TotalAmount
                    });
                }
            }

            return Ok(transactionHistoryWithProductDetails);
        }

        [HttpGet("getTransactionHistoryByUserId")]
        [Authorize(Roles = "buyer")]
        public async Task<IActionResult> GetTransactionHistoryByUserId(Guid id)
        {
            // Step 1: Filter transaction history by the provided BuyerId
            var transactionHistory = await _context.TransactionHistories
                .Where(t => t.BuyerId == id) // Filter by BuyerId
                .Select(t => new
                {
                    t.TransactionId,
                    t.ProductId,
                    t.Quantity,
                    t.TransactionDate,
                    t.TotalAmount
                })
                .ToListAsync();

            // Step 2: Fetch product details for each transaction and map to DTO
            var transactionHistoryWithProductDetails = new List<TransactionHistoryDTO>();

            foreach (var transaction in transactionHistory)
            {
                var product = await _context.Products
                    .Where(p => p.ProductId == transaction.ProductId)
                    .FirstOrDefaultAsync();

                if (product != null)
                {
                    transactionHistoryWithProductDetails.Add(new TransactionHistoryDTO
                    {
                        ProductName = product.ProductName,  // Get the ProductName
                        Quantity = transaction.Quantity,   // Map Quantity
                        TransactionDate = transaction.TransactionDate, // Map TransactionDate
                        TotalAmount = transaction.TotalAmount          // Map TotalAmount
                    });
                }
            }

            // Return the sorted transaction history
            return Ok(transactionHistoryWithProductDetails);
        }


    }
}
