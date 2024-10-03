using AutoMapper;
using InventoryService.Dtos;
using InventoryService.Models;

namespace InventoryService.Profiles
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<WarehouseDto, Warehouse>();

            CreateMap<Warehouse, WarehouseDto>();
        }
    }
}
