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
        Task<List<Product>> GetAllAsync();
    }
}
