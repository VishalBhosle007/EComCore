using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Application.DTOs
{
    public class UpdateProductRequestDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuentityInStock { get; set; }


        // For update flow:
        // - ExistingImageIds: IDs of ProductImage records that client wants to keep/update (meta)
        // - NewImagePaths: new uploaded image paths (returned by upload API)
        public List<int> ExistingImageIds { get; set; } = new();
        public List<string> NewImagePaths { get; set; } = new();

    }
}
