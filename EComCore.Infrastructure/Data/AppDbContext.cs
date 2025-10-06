using EComCore.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EComCore.Infrastructure.Identity;

namespace EComCore.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; } = default!;

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.Name).IsRequired().HasMaxLength(250);
                
                // Product with ProductImage (Cascade Delete)
                b.HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // cascade deletes DB rows

                // Product with category (Cascade Off)
                b.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(k => k.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<ProductImage>(b =>
            {
                b.HasKey(i => i.Id);
                b.Property(i => i.Path).IsRequired();
            });


            base.OnModelCreating(modelBuilder);

            #region Seed Product data
            var ProductList = new List<Product>()
            {
                new Product
                {
                    Id = Guid.Parse("52265969-5f3a-465f-9f96-9024929452aa"),
                    Name = "Product 1",
                    Description = "This is seeding form dbcontext file for trial",
                    Price = decimal.Parse("1200.00"),
                    QuentityInStock = 10
                },
                new Product
                {
                    Id = Guid.Parse("520a39f7-ab21-409d-9664-1ce731117ef6"),
                    Name = "Product 2",
                    Description = "This is seeding form dbcontext file for trial",
                    Price = decimal.Parse("1100.00"),
                    QuentityInStock = 5
                }
            };
            modelBuilder.Entity<Product>().HasData(ProductList);
            #endregion

        }
    }
}
