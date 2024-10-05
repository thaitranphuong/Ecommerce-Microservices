using AutoMapper;
using InventoryService.Dtos;
using InventoryService.Models;

namespace InventoryService.Profiles
{
    public class ImportDetailProfile : Profile
    {
        public ImportDetailProfile()
        {
            CreateMap<ImportDetailDto, ImportDetail>();

            CreateMap<ImportDetail, ImportDetailDto>();
        }
    }
}
