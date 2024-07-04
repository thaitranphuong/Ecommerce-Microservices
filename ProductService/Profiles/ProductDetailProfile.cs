using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Profiles
{
    public class ProductDetailProfile : Profile
    {
        public ProductDetailProfile()
        {
            CreateMap<ProductDetailDto, ProductDetail>()
                .ForMember(dest => dest.Image, src => src.MapFrom(x => x.Image))
                .ForMember(dest => dest.ProductId, src => src.MapFrom(x => x.ProductId));

            CreateMap<ProductDetail, ProductDetailDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Image, src => src.MapFrom(x => x.Image))
                .ForMember(dest => dest.ProductId, src => src.MapFrom(x => x.ProductId));
        }
    }
}
