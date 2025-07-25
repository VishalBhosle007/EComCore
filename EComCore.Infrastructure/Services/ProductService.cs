using EComCore.Application.Interfaces.Repository;
using EComCore.Application.Interfaces.Services;
using EComCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await productRepository.GetAllProducts();
        }
    }
}
