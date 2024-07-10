using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Dtos;
using ProductService.Models;
using ProductService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<bool> Save(UserDto dto)
        {
            var result = 0;
            var user = await _userRepository.FindById(dto.Id);
            if(user != null)
            {
                user.UserName = dto.UserName;
                user.Avatar = dto.Avatar;
                result = await _userRepository.SaveChange();
            }
            else 
                result = await _userRepository.CreateOne(_mapper.Map<User>(dto));
            return result > 0;
        }
    }
}
