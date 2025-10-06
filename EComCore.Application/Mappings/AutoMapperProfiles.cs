using AutoMapper;
using EComCore.Application.DTOs;
using EComCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Application.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //CreateMap<Product, ProductDto>().ReverseMap().ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.ImageUrl));
            //CreateMap<Product, InsertProductRequestDto>().ReverseMap();
            //CreateMap<Product, UpdateProductRequestDto>().ReverseMap();

            // Domain -> DTO
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<ProductImage, ProductImageDto>();

            // DTO -> Domain (when needed)
            CreateMap<ProductImageDto, ProductImage>();

            // Insert: ImagePaths (List<string>) -> Product.Images (ProductImage.Path)
            CreateMap<InsertProductRequestDto, Product>()
                .ForMember(dest => dest.Images,
                           opt => opt.MapFrom(src => src.ImagePaths.Select(url => new ProductImage { Path = url })));

            // Update: NewImagePaths -> Product.Images (only new images)
            CreateMap<UpdateProductRequestDto, Product>()
                .ForMember(dest => dest.Images,
                           opt => opt.MapFrom(src => src.NewImagePaths.Select(url => new ProductImage { Path = url })));


            //==========    Category    ==========

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, InsertCategoryRequestDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryRequestDto>().ReverseMap();

        }
    }
}
