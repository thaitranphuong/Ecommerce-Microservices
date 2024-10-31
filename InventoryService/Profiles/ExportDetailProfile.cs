using AutoMapper;
using InventoryService.Dtos;
using InventoryService.Models;

namespace InventoryService.Profiles
{
    public class ExportDetailProfile : Profile
    {
        public ExportDetailProfile()
        {
            CreateMap<ExportDetailDto, ExportDetail>();

            CreateMap<ExportDetail, ExportDetailDto>();
        }
    }
}
