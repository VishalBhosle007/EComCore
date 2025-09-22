using EComCore.Application.DTOs;
using EComCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(Guid Id);

        Task<ProductDto?> CreateProductAsync(InsertProductRequestDto product);

        Task<ProductDto?> UpdateProductAsync(Guid id, UpdateProductRequestDto product);

        Task<ProductDto?> DeleteProductAsync(Guid productId);
    }
}
