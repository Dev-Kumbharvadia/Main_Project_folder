using AppAPI.Data;
using AppAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> getAllTransaction()
        {
            return Ok();
        }

        [HttpGet("getAllTransactionById")]
        public async Task<IActionResult> getAllTransactionById(Guid Id)
        {
            return Ok();
        }

        [HttpPost("MakePurchase")]
        public async Task<IActionResult> MakePurchase(PurchaseDTO purchase)
        {
            return Ok();
        }
    }
}
