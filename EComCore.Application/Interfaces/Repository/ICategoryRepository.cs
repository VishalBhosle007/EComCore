using EComCore.Application.DTOs;
using EComCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Application.Interfaces.Repository
{
    public interface ICategoryRepository
    {
        Task<Category?> CreateCategoryAsync(Category category);
        Task<List<Category>> GetAllCategoryAsync();
        Task<Category?> GetCategoryByIdAsync(Guid categoryId);
        Task<Category?> UpdateCategoryAsync(Category caregory);
        Task<Category?> DeleteCategoryAsync(Guid categoryId);

    }
}
