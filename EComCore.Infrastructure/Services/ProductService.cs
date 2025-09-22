using AutoMapper;
using EComCore.Application.DTOs;
using EComCore.Application.Exceptions;
using EComCore.Application.Interfaces.Repository;
using EComCore.Application.Interfaces.Services;
using EComCore.Domain.Entities;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace EComCore.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorage;

        public ProductService(IProductRepository productRepository, IMapper mapper, IFileStorageService fileStorage)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.fileStorage = fileStorage; // used to delete files
        }

        public async Task<ProductDto?> CreateProductAsync(InsertProductRequestDto addProductRequestDto)
        {
            #region Input Validation 
            if (addProductRequestDto == null)
            {
                throw new ValidationException("Product can not be null");
            }
            if (string.IsNullOrWhiteSpace(addProductRequestDto.Name))
                throw new ValidationException("Product name is required.");

            if (addProductRequestDto.Price <= 0)
                throw new ValidationException("Product price must be greater than zero.");

            if (addProductRequestDto.QuentityInStock < 0)
                throw new ValidationException("Quantity cannot be negative.");
            #endregion

            var product = mapper.Map<Product>(addProductRequestDto);

            // Map Images - if ImagePaths exist, mapper already did it; keep defensive check
            if (addProductRequestDto.ImagePaths != null && addProductRequestDto.ImagePaths.Any())
            {
                product.Images = addProductRequestDto.ImagePaths
                    .Select(url => new ProductImage { Path = url })
                    .ToList();
            }

            var result = await productRepository.CreateProductAsync(product);

            if (result == null)
                throw new ValidationException("Something went wrong while creating the product.");

            return mapper.Map<ProductDto>(result);
        }

        public async Task<ProductDto> DeleteProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ValidationException("ProductId cannot be empty.");
            }

            var result = await productRepository.DeleteProductByIdAsync(productId);

            if (result == null)
            {
                throw new NotFoundException($"Product with id {productId} was not found.");
            }

            // delete physical files after DB deletion (result still has images)
            foreach (var img in result.Images)
            {
                if (!string.IsNullOrWhiteSpace(img.Path))
                {
                    await fileStorage.DeleteFileAsync(img.Path);
                }
            }

            return mapper.Map<ProductDto>(result);
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await productRepository.GetAllProductsAsync();
            return mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(Guid Id)
        {
            if (Id == Guid.Empty)
                throw new ValidationException("ProductId cannot be empty.");

            var product = await productRepository.GetProductByIdAsync(Id);

            if (product == null)
                throw new NotFoundException($"Product with id {Id} was not found.");

            return mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductRequestDto productDto)
        {
            #region Input Validation
            if (id == Guid.Empty)
                throw new ValidationException("ProductId cannot be empty.");

            if (productDto == null)
                throw new ValidationException("Product data is required.");

            if (productDto.Price <= 0)
                throw new ValidationException("Product price must be greater than zero.");

            if (productDto.QuentityInStock < 0)
                throw new ValidationException("Quantity cannot be negative.");
            #endregion

            // 1) Fetch existing product (with images)
            var existing = await productRepository.GetProductByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Product with id {id} was not found.");

            // 2) Determine which images to keep and which to remove
            var keepIds = productDto.ExistingImageIds ?? new List<int>();

            var imagesToRemove = existing.Images
                .Where(img => !keepIds.Contains(img.Id))
                .ToList();

            // Delete physical files for removed images
            foreach (var img in imagesToRemove)
            {
                if (!string.IsNullOrWhiteSpace(img.Path))
                {
                    await fileStorage.DeleteFileAsync(img.Path);
                }
            }

            // 3) Build a product entity representing the desired final state (preserve Ids for kept images)
            var keptImages = existing.Images
                .Where(img => keepIds.Contains(img.Id))
                .Select(img => new ProductImage
                {
                    Id = img.Id,
                    Path = img.Path,
                    ProductId = existing.Id
                })
                .ToList();

            var newImages = (productDto.NewImagePaths ?? new List<string>())
                .Distinct()
                .Select(url => new ProductImage
                {
                    // Id = 0 (default) -> repository will treat as new
                    Path = url
                })
                .ToList();

            var productEntity = new Product
            {
                Id = existing.Id,
                Description = productDto.Description,
                Price = productDto.Price,
                QuentityInStock = productDto.QuentityInStock,
                Images = keptImages.Concat(newImages).ToList()
            };

            // 4) Persist via repository (repository will sync images: remove omitted, add new, update paths)
            var result = await productRepository.UpdateProductAsync(productEntity);

            if (result == null)
                throw new NotFoundException($"Product with id {id} was not found.");

            return mapper.Map<ProductDto>(result);
        }
    }
}
