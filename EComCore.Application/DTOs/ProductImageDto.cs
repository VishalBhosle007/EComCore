using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Application.DTOs
{
    public class ProductImageDto
    {
        public int Id { get; set; } // 0 => new image (if created client-side)
        public string Path { get; set; } = default!;
        public string? AltText { get; set; }

    }
}
