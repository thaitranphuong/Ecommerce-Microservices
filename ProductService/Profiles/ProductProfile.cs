using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>()
               .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
               .ForMember(dest => dest.ShortDescription, src => src.MapFrom(x => x.ShortDescription))
               .ForMember(dest => dest.FullDescription, src => src.MapFrom(x => x.FullDescription))
               .ForMember(dest => dest.Thumbnail, src => src.MapFrom(x => x.Thumbnail))
               .ForMember(dest => dest.Price, src => src.MapFrom(x => x.Price))
               .ForMember(dest => dest.CategoryId, src => src.MapFrom(x => x.CategoryId));

            CreateMap<Product, ProductDto>()
               .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
               .ForMember(dest => dest.ShortDescription, src => src.MapFrom(x => x.ShortDescription))
               .ForMember(dest => dest.FullDescription, src => src.MapFrom(x => x.FullDescription))
               .ForMember(dest => dest.Thumbnail, src => src.MapFrom(x => x.Thumbnail))
               .ForMember(dest => dest.Price, src => src.MapFrom(x => x.Price))
               .ForMember(dest => dest.Enabled, src => src.MapFrom(x => x.Enabled))
               .ForMember(dest => dest.CategoryId, src => src.MapFrom(x => x.CategoryId));
        }
    }
}
