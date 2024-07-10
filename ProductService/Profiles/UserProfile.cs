using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Profiles
{
    public class UserProfile : Profile
    {
       public UserProfile()
        {
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.UserName))
                .ForMember(dest => dest.Avatar, src => src.MapFrom(x => x.Avatar));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.UserName))
                .ForMember(dest => dest.Avatar, src => src.MapFrom(x => x.Avatar));
        }
    }
}
