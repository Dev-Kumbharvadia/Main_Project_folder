﻿using AppAPI.Data;
using AppAPI.Models.Domain;
using AppAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Numerics;
using TodoAPI.Models;

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

        [HttpGet("GetSortedProduct")]
        [Authorize]
        public async Task<IActionResult> GetSortedProduct([FromQuery] SieveModel model)
        {
            IQueryable<Product> productQuery;

            if (model.Filters == null)
            {
                // If no filters are provided, fetch all products and include Seller details
                productQuery = _context.Products
                    .Include(p => p.Seller)
                    .AsQueryable()
                    .Where(p => !_context.DeletedProducts
                        .Any(dp => dp.ProductId == p.ProductId));
            }
            else
            {
                // If filters are provided, apply filter for product name containing the filter text
                productQuery = _context.Products
                    .Include(p => p.Seller)
                    .Where(p => p.ProductName.Contains(model.Filters))
                    .Where(p => !_context.DeletedProducts
                        .Any(dp => dp.ProductId == p.ProductId))
                    .AsQueryable();
            }

            // Apply filtering and sorting using Sieve
            productQuery = _sieveProcessor.Apply(model, productQuery);

            // Ensure default values for paging
            if (model.Page == null || model.PageSize == null)
            {
                model.Page = 1;
                model.PageSize = 5;
            }

            int totalCount = model.Sorts == null && model.Filters == null
                ? await _context.Products.CountAsync()
                : await productQuery.CountAsync();

            // Fix the totalPages calculation
            var totalPages = (int)Math.Ceiling((double)totalCount / model.PageSize.Value);

            // Handle page and pageSize defaults
            
            int page = model.Page.HasValue ? model.Page.Value : 1;
            int pageSize = model.PageSize.HasValue ? model.PageSize.Value : 10;

            // Apply paging
            var products = await productQuery
                .OrderBy(p => p.ProductId) // Ensure stable ordering
                .Skip(0)
                .Take(pageSize)
                .ToListAsync();


            // Handle empty results
            if (products == null || !products.Any())
            {
                return Ok(new ApiResponse<object>
                {
                    Message = "no product found",
                    Success = true,
                });
            }

            // Format the result
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

            // Return the response
            return Ok(new ApiResponse<object>
            {
                Message = "Products retrieved successfully",
                Success = true,
                Data = new
                {
                    result,
                    totalPages = totalPages,
                    currentPage = page
                }
            });
        }

        [HttpGet("getSortedProductsBySellerId")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> GetSortedProductsBySellerId([FromQuery] SieveModel model, Guid SellerId)
        {
            var productQuery = _context.Products
                .Include(p => p.Seller)
                .Where(p => p.SellerId == SellerId)
                .Where(p => !_context.DeletedProducts
                        .Any(dp => dp.ProductId == p.ProductId))
                .AsQueryable();

            // Apply filtering and sorting using Sieve
            productQuery = _sieveProcessor.Apply(model, productQuery);

            // Ensure default values for paging
            if (model.Page == null || model.PageSize == null)
            {
                model.Page = 1;
                model.PageSize = 5;
            }

            int totalCount = model.Sorts == null && model.Filters == null
                ? await _context.Products.CountAsync()
                : await productQuery.CountAsync();

            // Fix the totalPages calculation
            var totalPages = (int)Math.Ceiling((double)totalCount / model.PageSize.Value);

            // Handle page and pageSize defaults

            int page = model.Page.HasValue ? model.Page.Value : 1;
            int pageSize = model.PageSize.HasValue ? model.PageSize.Value : 10;

            // Apply paging
            var products = await productQuery
                .OrderBy(p => p.ProductId) // Ensure stable ordering
                .Skip(0)
                .Take(pageSize)
                .ToListAsync();


            // Handle empty results
            if (products == null || !products.Any())
            {
                return NotFound(new { message = "No Products found." });
            }

            // Format the result
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

            // Return the response
            return Ok(new ApiResponse<object>
            {
                Message = "Products retrieved successfully",
                Success = true,
                Data = new
                {
                    result,
                    totalPages = totalPages,
                    currentPage = page
                }
            });
        }

        // GET: api/Product/{id}
        [HttpGet("GetProductById")]
        [Authorize]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Ok(product);
        }

        [HttpPost("AddProduct")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> AddProduct([FromForm] ProductUploadDTO productDto)
        {
            // Validate if the image file is provided
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
        [Authorize(Roles = "seller")]
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

            product.UpdatedAt = DateTime.UtcNow;

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

    [HttpDelete("DeleteProduct")] //ok
        [Authorize(Roles = "seller")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {

        var deletedProduct = _context.DeletedProducts.Add( new DeletedProducts { Id = Guid.NewGuid(), ProductId = id });
        try
        {;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while deleting product: {ex.Message}");
            return StatusCode(500, new { message = $"Internal server error, unable to delete product {ex}" });
        }
    }
}
}
