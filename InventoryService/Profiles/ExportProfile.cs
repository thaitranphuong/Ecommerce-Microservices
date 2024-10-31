using AutoMapper;
using InventoryService.Dtos;
using InventoryService.Models;
using System;

namespace InventoryService.Profiles
{
    public class ExportProfile : Profile
    {
        public ExportProfile()
        {
            CreateMap<ExportDto, Export>()
                .ForMember(dest => dest.CreatedTime, src => src.MapFrom(x => DateTime.Now));

            CreateMap<Export, ExportDto>();
        }
    }
}
