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

        public async Task<List<Product>> GetAllProducts()
        {
            return await dbContext.Products.ToListAsync();
        }
       
    }
}
