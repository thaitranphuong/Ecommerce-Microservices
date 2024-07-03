using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public interface IFileStorageService
    {
        Task<string> Upload(string directory, IFormFile file);
    }
}
