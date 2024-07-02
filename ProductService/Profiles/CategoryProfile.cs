using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code));

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code));
        }
    }
}
