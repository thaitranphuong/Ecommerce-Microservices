using AutoMapper;
using IdentityService.Helpers;
using IdentityService.Models;
using IdentityService.Models.DTOs;
using IdentityService.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace IdentityService.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Create(UserDto user)
        {
            var existedUser = await _userRepository.FindByEmail(user.Email);
            if (existedUser != null)
                return new UserDto() { Roles = new List<string> { "customer" }};
            user.Id = Guid.NewGuid().ToString();
            user.Password = PasswordHelper.HashPassword(user.Password);
            bool result = await _userRepository.CreateOne(_mapper.Map<User>(user));
            if (result == true)
            {
                user.Password = "";
                user.Roles = new List<string> { "customer" };
                return user;
            }
            return null;
        }

        public async Task<UserDto> FindByEmail(string email)
        {
            return _mapper.Map<UserDto>(await _userRepository.FindByEmail(email));
        }

        public async Task<UserDto> Login(string email, string password)
        {
            var user = await _userRepository.FindByEmail(email);
            if (user == null)
                return null;
            else
            {
                var isMatch = Helpers.PasswordHelper.VerifyPassword(password, user.Password);
                if (isMatch)
                {
                    var dto = _mapper.Map<UserDto>(user);
                    dto.Roles = new List<string> { "customer" };
                    if (user.Roles.Count == 2)
                        dto.Roles.Add("admin");
                    return dto;
                }
                else return null;
            }
        }
    }
}
