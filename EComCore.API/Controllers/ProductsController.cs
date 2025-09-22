using Azure.Core;
using EComCore.API.Filters;
using EComCore.Application.DTOs;
using EComCore.Application.Interfaces.Services;
using EComCore.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace EComCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IFileStorageService fileStorage;

        public ProductsController(IProductService productService, IFileStorageService fileStorage)
        {
            this.productService = productService;
            this.fileStorage = fileStorage;
        }

        //[Authorize(Roles = "Customer,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productService.GetAllAsync();
            return Ok(products);
        }

        //[Authorize(Roles = "Customer,Admin")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await productService.GetByIdAsync(id);
            return Ok(product);
        }

        // Upload images endpoint: client uploads files first, gets back paths
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files)
        {
            if (files == null || !files.Any())
                return BadRequest(new { Message = "No files provided." });

            var folder = "uploads/products";
            var saved = new List<string>();

            foreach (var file in files)
            {
                var path = await fileStorage.SaveFileAsync(file, folder);
                saved.Add(path);
            }

            return Ok(saved);
        }

        // Create product (client should pass InsertProductRequestDto with ImagePaths returned by upload)
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] InsertProductRequestDto dto)
        {
            var product = await productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // Update product
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequestDto dto)
        {
            if (id == Guid.Empty)
                return BadRequest(new { Message = "Product ID cannot be empty." });

            if (dto == null)
                return BadRequest(new { Message = "Product data is required." });

            if (id != dto.Id)
                return BadRequest(new { Message = "Route id and body id do not match." });

            var updated = await productService.UpdateProductAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { Message = "Product ID cannot be empty." });

            var result = await productService.DeleteProductAsync(id);
            return Ok(result);
        }
    }

}

