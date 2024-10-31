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
               .ForMember(dest => dest.Enabled, src => src.MapFrom(x => x.Enabled))
               .ForMember(dest => dest.Expiry, src => src.MapFrom(x => x.Expiry))
               .ForMember(dest => dest.DiscountPercent, src => src.MapFrom(x => x.DiscountPercent))
               .ForMember(dest => dest.Origin, src => src.MapFrom(x => x.Origin))
               .ForMember(dest => dest.CategoryId, src => src.MapFrom(x => x.CategoryId))
               .ForMember(dest => dest.UnitId, src => src.MapFrom(x => x.UnitId));

            CreateMap<Product, ProductDto>()
               .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
               .ForMember(dest => dest.Expiry, src => src.MapFrom(x => x.Expiry))
               .ForMember(dest => dest.DiscountPercent, src => src.MapFrom(x => x.DiscountPercent))
               .ForMember(dest => dest.Origin, src => src.MapFrom(x => x.Origin))
               .ForMember(dest => dest.ShortDescription, src => src.MapFrom(x => x.ShortDescription))
               .ForMember(dest => dest.FullDescription, src => src.MapFrom(x => x.FullDescription))
               .ForMember(dest => dest.Thumbnail, src => src.MapFrom(x => x.Thumbnail))
               .ForMember(dest => dest.Price, src => src.MapFrom(x => x.Price - x.Price * x.DiscountPercent / 100))
               .ForMember(dest => dest.OldPrice, src => src.MapFrom(x => x.Price))
               .ForMember(dest => dest.Enabled, src => src.MapFrom(x => x.Enabled))
               .ForMember(dest => dest.CategoryId, src => src.MapFrom(x => x.CategoryId))
               .ForMember(dest => dest.CategoryName, src => src.MapFrom(x => x.Category.Name))
               .ForMember(dest => dest.UnitId, src => src.MapFrom(x => x.Unit_.Id))
               .ForMember(dest => dest.Unit, src => src.MapFrom(x => x.Unit_.Name))
               .ForMember(dest => dest.Comments, src => src.MapFrom(x => x.Comments));
        }
    }
}
