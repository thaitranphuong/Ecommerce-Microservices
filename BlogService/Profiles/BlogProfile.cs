using AutoMapper;
using BlogService.Dtos;
using BlogService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Profiles
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<BlogDto, Blog>();

            CreateMap<Blog, BlogDto>();
        }
    }
}
