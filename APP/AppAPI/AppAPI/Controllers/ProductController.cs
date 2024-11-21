using AppAPI.Data;
using AppAPI.Models.Domain;
using AppAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly SieveProcessor _sieveProcessor;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment environment, SieveProcessor sieveProcessor)
        {
            _context = context;
            _environment = environment;
            _sieveProcessor = sieveProcessor;
        }

        // GET: api/Product/GetAllProducts
        [HttpGet("GetAllProducts")] //ok
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _context.Products
                .Include(p => p.Seller) // Eagerly load the Seller navigation property
                .ToListAsync();

            if (products == null || !products.Any())
            {
                return NotFound(new { message = "No Products found." });
            }

            // Transform data to include Seller details
            var result = products.Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.Description,
                p.ImageContent,
                p.Price,
                p.StockQuantity,
                p.CreatedAt,
                p.UpdatedAt,
                Seller = new
                {
                    p.Seller.UserId,
                    p.Seller.Username,
                    p.Seller.Email
                }
            });

            return Ok(result);
        }


        // Read Sort
        [HttpGet("Sorted")] //ok
        public async Task<IActionResult> GetSortedProduct([FromQuery] SieveModel model)
        {
            var ProductQuery = _context.Products.AsQueryable();

            ProductQuery = _sieveProcessor.Apply(model, ProductQuery);

            var Product = await ProductQuery.ToListAsync();

            return Ok(Product);
        }

        // GET: api/Product/{id}
        [HttpGet("GetProductById")] //ok
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Ok(product);
        }

        // POST: api/Product/upload
        [HttpPost("AddProduct")] //ok
        public async Task<IActionResult> AddProduct([FromForm] ProductUploadDTO productDto)
        {
            if (productDto.ImageFile == null || productDto.ImageFile.Length == 0)
                return BadRequest("No file uploaded.");

            byte[] fileContent;
            using (var memoryStream = new MemoryStream())
            {
                await productDto.ImageFile.CopyToAsync(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            var product = new Product
            {
                ProductName = productDto.ProductName,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                Description = productDto.Description,
                ImageContent = fileContent,
                SellerId = productDto.SellerId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return Ok(new { product.ProductId, product.ProductName });
        }

        // PUT: api/Product/{id}
        [HttpPut("UpdateProduct")] //ok
        public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] ProductUpdateDTO productDto)
        {
            // Fetch the product by ID
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found." });

            // Check and update the product properties only if they are provided
            if (!string.IsNullOrEmpty(productDto.ProductName))
            {
                product.ProductName = productDto.ProductName;
            }

            if (productDto.Price != null && productDto.Price != product.Price)
            {
                product.Price = productDto.Price;
            }

            if (productDto.StockQuantity != null && productDto.StockQuantity != product.StockQuantity)
            {
                product.StockQuantity = productDto.StockQuantity;
            }

            if (!string.IsNullOrEmpty(productDto.Description))
            {
                product.Description = productDto.Description;
            }

            // Always update the UpdatedAt field
            product.UpdatedAt = DateTime.UtcNow;

            // Handle file upload if provided
            if (productDto.File != null && productDto.File.Length > 0)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await productDto.File.CopyToAsync(memoryStream);
                        product.ImageContent = memoryStream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Error processing the image file.", error = ex.Message });
                }
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product updated successfully.", product = product });
        }

        // DELETE: api/Product/{id}
        [HttpDelete("DeleteProduct")] //ok
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting product: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error, unable to delete product" });
            }
        }

    }
}
