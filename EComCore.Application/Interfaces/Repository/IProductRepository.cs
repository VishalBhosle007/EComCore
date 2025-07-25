using EComCore.Application.DTOs;
using EComCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Application.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
    }
}
