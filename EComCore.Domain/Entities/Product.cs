
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EComCore.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int QuentityInStock { get; set; }

        // category
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        // Navigation
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}

