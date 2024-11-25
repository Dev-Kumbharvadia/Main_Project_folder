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
        [Authorize(Roles="buyer")]
        public async Task<IActionResult> MakePurchase(PurchaseDTO purchase)
        {
            // Validate the input
            if (purchase == null || purchase.Quantity <= 0 || purchase.ProductId == Guid.Empty || purchase.BuyerId == Guid.Empty)
            {
                return BadRequest("Invalid purchase details.");
            }

            var product = await _context.Products
                .Where(p => p.ProductId == purchase.ProductId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            if (product.StockQuantity < purchase.Quantity)
            {
                return BadRequest("Not enough stock available.");
            }

            double totalAmount = product.Price * purchase.Quantity;

            var transactionHistory = new TransactionHistory
            {
                TransactionId = Guid.NewGuid(),
                ProductId = purchase.ProductId,
                BuyerId = purchase.BuyerId,
                Quantity = purchase.Quantity,
                TotalAmount = totalAmount,
                TransactionDate = DateTime.UtcNow
            };

            // Step 5: Save the transaction to the database
            _context.TransactionHistories.Add(transactionHistory);

            // Step 6: Update the product stock
            product.StockQuantity -= purchase.Quantity;
            _context.Products.Update(product);

            // Step 7: Save changes to the database
            await _context.SaveChangesAsync();

            // Step 8: Return a success response with transaction details
            return Ok(new { Message = "Purchase successful.", TransactionId = transactionHistory.TransactionId });
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
