using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentDto, Comment>();

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.User.UserName))
                .ForMember(dest => dest.LikeCount, src => src.MapFrom(x => x.Likes.Count))
                .ForMember(dest => dest.UserAvatar, src => src.MapFrom(x => x.User.Avatar));
        } 
    }
}
