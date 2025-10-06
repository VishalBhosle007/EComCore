using EComCore.Application.DTOs;
using EComCore.Application.Interfaces.Repository;
using EComCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto?> CreateCategoryAsync(InsertCategoryRequestDto categoryDto);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetByIdAsync(Guid categoryId);

        Task<CategoryDto?> UpdateCategoryAsync(Guid id, UpdateCategoryRequestDto categoryRequest);

        Task<CategoryDto?> DeleteCategoryAsync(Guid categoryId);
    }
}
