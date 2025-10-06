using EComCore.Application.DTOs;
using EComCore.Application.Interfaces.Repository;
using EComCore.Domain.Entities;
using EComCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product?> CreateProductAsync(Product addProductRequest)
        {
            await dbContext.Products.AddAsync(addProductRequest);
            await dbContext.SaveChangesAsync();
            return addProductRequest;
        }

        public async Task<Product?> DeleteProductByIdAsync(Guid productId)
        {
            if (productId == Guid.Empty) return null;

            // include images so caller can delete physical files afterward
            var product = await dbContext.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(a => a.Id == productId);

            if (product == null) return null;

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await dbContext.Products.Include(c => c.Category).Include(p => p.Images).ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            if (productId == Guid.Empty)
                return null;

            return await dbContext.Products.Include(c => c.Category).Include(p => p.Images)
                .FirstOrDefaultAsync(i => i.Id == productId);
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            // product parameter represents the desired final state:
            // - product.Images contains kept images (with Id) and new images (Id = 0)
            var existing = await dbContext.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(a => a.Id == product.Id);

            if (existing == null) return null;

            // Update scalar props
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.QuentityInStock = product.QuentityInStock;
            existing.CategoryId = product.CategoryId;

            // Determine image ids that should remain (those passed with Id != 0)
            var updatedImageIds = product.Images?.Where(i => i.Id != 0).Select(i => i.Id).ToList()
                                  ?? new List<int>();

            // 1) Remove DB image rows that are not in updatedImageIds
            var imagesToRemove = existing.Images.Where(i => !updatedImageIds.Contains(i.Id)).ToList();
            if (imagesToRemove.Any())
            {
                foreach (var img in imagesToRemove)
                {
                    dbContext.ProductImages.Remove(img);
                }
            }

            // 2) Add new images (Id == 0) and update existing images' paths if changed
            foreach (var img in product.Images)
            {
                if (img.Id == 0)
                {
                    // new image
                    existing.Images.Add(new ProductImage
                    {
                        Path = img.Path,
                        ProductId = existing.Id
                    });
                }
                else
                {
                    // update existing image path if needed
                    var existImg = existing.Images.FirstOrDefault(x => x.Id == img.Id);
                    if (existImg != null)
                    {
                        existImg.Path = img.Path;
                    }
                }
            }

            await dbContext.SaveChangesAsync();
            return existing;
        }

    }
}
