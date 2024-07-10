using ProductService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public interface IUserService
    {
        Task<bool> Save(UserDto user);
    }
}
