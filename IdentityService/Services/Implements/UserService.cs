using AutoMapper;
using IdentityService.Helpers;
using IdentityService.Models;
using IdentityService.Models.Dtos.Pagination;
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

        public async Task<UserOutput> FindAll(string email, int page, int limit)
        {
            var userList = await _userRepository.FindAll(email, page, limit);
            var userDtoList = new List<UserDto>();
            foreach (var user in userList)
            {
                var roleList = new List<string>();
                foreach (var role in user.Roles)
                {
                    roleList.Add(role.Name);
                }
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = roleList;
                userDtoList.Add(userDto);
            }

            UserOutput output = new UserOutput();
            output.Name = email;
            output.Page = page;
            output.TotalPage = (int)Math.Ceiling((double)(await _userRepository.FindAllNoPagination(email)).Count / limit);
            output.ListResult = userDtoList;
            return output;
        }

        public async Task<List<UserDto>> FindAllAdmin()
        {
            var userList = await _userRepository.FindAllAdmin();
            var userDtoList = new List<UserDto>();
            foreach (var user in userList)
            {
                var roleList = new List<string>();
                foreach (var role in user.Roles)
                {
                    roleList.Add(role.Name);
                }
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = roleList;
                userDtoList.Add(userDto);
            }
            return userDtoList;
        }

        public async Task<List<UserDto>> FindAllCustomer()
        {
            var userList = await _userRepository.FindAllCustomer();
            var userDtoList = new List<UserDto>();
            foreach (var user in userList)
            {
                var roleList = new List<string>();
                foreach (var role in user.Roles)
                {
                    roleList.Add(role.Name);
                }
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = roleList;
                userDtoList.Add(userDto);
            }
            return userDtoList;
        }

        public async Task<List<UserDto>> FindAllNoPagination()
        {
            var userList = await _userRepository.FindAllNoPagination(null);
            var userDtoList = new List<UserDto>();
            foreach (var user in userList)
            {
                var roleList = new List<string>();
                foreach (var role in user.Roles)
                {
                    roleList.Add(role.Name);
                }
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = roleList;
                userDtoList.Add(userDto);
            }
            return userDtoList;
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
