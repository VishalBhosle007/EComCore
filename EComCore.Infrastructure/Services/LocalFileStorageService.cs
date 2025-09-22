using EComCore.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
namespace EComCore.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IHostEnvironment _env; // ✅ IHostEnvironment works in class library
        private readonly ILogger<LocalFileStorageService> _logger;

        public LocalFileStorageService(IHostEnvironment env, ILogger<LocalFileStorageService> logger = null)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            var uploadFolder = Path.Combine(_env.ContentRootPath, "wwwroot", folder);
            if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var relative = Path.Combine("/", folder.Replace("\\", "/"), fileName).Replace("\\", "/");
            return relative;
        }

        public Task DeleteFileAsync(string path)
        {
            try
            {
                var relative = path.TrimStart('/');
                var physical = Path.Combine(_env.ContentRootPath, "wwwroot", relative.Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (File.Exists(physical)) File.Delete(physical);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to delete file: {Path}", path);
            }
            return Task.CompletedTask;
        }
    }
}
