using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Domain.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; } // FK
        public string Path { get; set; } = default!; // relative or absolute URL stored
        public string? AltText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Navigation
        public Product? Product { get; set; }

    }
}
