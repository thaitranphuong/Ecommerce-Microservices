﻿using AutoMapper;
using IdentityService.Models;
using IdentityService.Models.DTOs;

namespace IdentityService.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.IsAdmin, src => src.MapFrom(x => x.Roles.Count == 2))
                .ForMember(dest => dest.Password, src => src.MapFrom(x => ""));
        }
    }
}
