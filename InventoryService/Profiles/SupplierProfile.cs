using AutoMapper;
using InventoryService.Dtos;
using InventoryService.Models;

namespace InventoryService.Profiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<SupplierDto, Supplier>();

            CreateMap<Supplier, SupplierDto>();
        }
    }
}
