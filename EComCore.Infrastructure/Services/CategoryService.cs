using AutoMapper;
using EComCore.Application.DTOs;
using EComCore.Application.Interfaces.Repository;
using EComCore.Application.Interfaces.Services;
using EComCore.Domain.Entities;
using EComCore.Infrastructure.Data;
using EComCore.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EComCore.Application.Exceptions;
using Microsoft.Identity.Client;

namespace EComCore.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<CategoryDto?> CreateCategoryAsync(InsertCategoryRequestDto categoryRequest)
        {
            if (categoryRequest == null)
            {
                throw new ValidationException("Category can not be null");
            }
            if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            {
                throw new ValidationException("Category name is must");
            }

            // map domail to dto
            var category = mapper.Map<Category>(categoryRequest);

            var result = await categoryRepository.CreateCategoryAsync(category);

            if (result == null)
            {
                throw new ValidationException("Something went wrong while creating the category.");
            }

            return mapper.Map<CategoryDto>(result);

        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await categoryRepository.GetAllCategoryAsync();
            return mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ValidationException("Please enter categoryId");
            }

            var category = await categoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                throw new ValidationException($"Category with id {id} was not found");
            }
            return mapper.Map<CategoryDto?>(category);

        }

        public async Task<CategoryDto?> UpdateCategoryAsync(Guid id, UpdateCategoryRequestDto categoryRequest)
        {
            if (id == Guid.Empty)
            {
                throw new ValidationException("Category id can not be empty");
            }

            if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            {
                throw new ValidationException("Category name is must");
            }

            var request = mapper.Map<Category>(categoryRequest);
            request.Id = id;

            var result = await categoryRepository.UpdateCategoryAsync(request);
            if (result == null)
            {
                throw new NotFoundException($"Category  with id {id} was not found.");
            }

            return mapper.Map<CategoryDto>(result);
        }

        public async Task<CategoryDto?> DeleteCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new ValidationException("Category ID can not be empty.");


            var category = await categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
                throw new NotFoundException($"Category with id {categoryId} was not found.");


            var result = await categoryRepository.DeleteCategoryAsync(categoryId);
            if (result == null)
                throw new ValidationException("Something wen wrong while deleting category.");

            return mapper.Map<CategoryDto>(result);
        }

    }
}
