using EComCore.Application.Interfaces.Repository;
using EComCore.Domain.Entities;
using EComCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category?> CreateCategoryAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<List<Category>> GetAllCategoryAsync()
        {
            return await dbContext.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            var category = await dbContext.Categories.FirstOrDefaultAsync(a => a.Id == id);
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(Category category)
        {
            var existing = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (existing == null)
                return null;

            existing.Name = category.Name;
            existing.Description = category.Description;
            existing.ImageUrl = category.ImageUrl;
            existing.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<Category?> DeleteCategoryAsync(Guid categoryId)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(a => a.Id == categoryId);
            if (category == null)
                return null;

            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();

            return category;
        }

    }
}
