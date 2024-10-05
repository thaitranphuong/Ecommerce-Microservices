using AutoMapper;
using InventoryService.Dtos;
using InventoryService.Models;
using System;

namespace InventoryService.Profiles
{
    public class ImportProfile : Profile
    {
        public ImportProfile()
        {
            CreateMap<ImportDto, Import>()
                .ForMember(dest => dest.CreatedTime, src => src.MapFrom(x => DateTime.Now));

            CreateMap<Import, ImportDto>();
        }
    }
}
