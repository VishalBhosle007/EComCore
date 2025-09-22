using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        // Save returns a relative or absolute path/URL for DB
        Task<string> SaveFileAsync(IFormFile file, string folder);
        Task DeleteFileAsync(string path);

    }
}
