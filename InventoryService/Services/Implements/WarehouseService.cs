using AutoMapper;
using InventoryService.Dtos;
using InventoryService.Dtos.Pagination;
using InventoryService.Models;
using InventoryService.Repositories;
using InventoryService.SyncServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryService.Services.Implements
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IImportRepository _importRepository;
        private readonly IGrpcOrderDetailService _grpcOrderDetailService;
        private readonly IGrpcProductService _grpcProductService;
        private readonly IMapper _mapper;

        public WarehouseService(IWarehouseRepository warehouseRepository, IMapper mapper, IImportRepository importRepository, IGrpcOrderDetailService grpcOrderDetailService, IGrpcProductService grpcProductService)
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
            _importRepository = importRepository;
            _grpcOrderDetailService = grpcOrderDetailService;
            _grpcProductService = grpcProductService;
        }

        public async Task<bool> Save(WarehouseDto dto)
        {
            var result = false;
            if (dto.Id == 0)
            {
                result = await _warehouseRepository.CreateOne(_mapper.Map<Warehouse>(dto)) > 0;
            }
            else
            {
                Warehouse warehouse = await _warehouseRepository.FindById(dto.Id);
                warehouse.Name = dto.Name;
                warehouse.Address = dto.Address;
                result = await _warehouseRepository.SaveChange() > 0;
            }
            return result;
        }

        public async Task<WarehouseDto> FindById(int id)
        {
            var warehouseDto = _mapper.Map<WarehouseDto>(await _warehouseRepository.FindById(id));
            return warehouseDto;
        }

        public async Task<WarehouseOutput> FindAll(string name, int page, int limit)
        {
            var warehouses = await _warehouseRepository.FindAll(name, page, limit);
            var warehouseDtos = new List<WarehouseDto>();
            foreach (var warehouse in warehouses)
            {
                warehouseDtos.Add(_mapper.Map<WarehouseDto>(warehouse));
            }
            WarehouseOutput output = new WarehouseOutput();
            output.Name = name;
            output.Page = page;
            output.TotalPage = (int) Math.Ceiling((double)(await _warehouseRepository.FindByName(name)).Count / limit);
            output.ListResult = warehouseDtos;
            return output;
        }

        public async Task<bool> DeleteById(int id)
        {
            var warehouse = await _warehouseRepository.FindById(id);
            if (warehouse.Imports.Count > 0)
                return false;
            return await _warehouseRepository.Remove(warehouse) > 0;
        }

        public async Task<List<InStockProductDto>> FindAllInstock(int id)
        {
            var imports = await _importRepository.FindByWarehouseId(id);
            var importProducts = new Dictionary<int, int>();
            var exportProducts = new Dictionary<int, int>();
            var remainingProducts = new Dictionary<int, int>();
            var expiredProducts = new Dictionary<int, int>();


            foreach (var import in imports)
            {
                var importDetails = import.ImportDetails;
                foreach (var importDetail in importDetails)
                {
                    var productId = importDetail.ProductId;
                    if (!importProducts.ContainsKey(productId))
                    {
                        importProducts.Add(productId, importDetail.Quantity);
                    }
                    else
                    {
                        importProducts[productId] += importDetail.Quantity;
                    }
                    if (!exportProducts.ContainsKey(productId))
                    {
                        exportProducts.Add(productId, importDetail.ExportedQuantity);
                    }
                    else
                    {
                        exportProducts[productId] += importDetail.ExportedQuantity;
                    }
                    if (!remainingProducts.ContainsKey(productId))
                    {
                        remainingProducts.Add(productId, importDetail.Quantity - importDetail.ExportedQuantity);
                    }
                    else
                    {
                        remainingProducts[productId] += (importDetail.Quantity - importDetail.ExportedQuantity);
                    }
                }
            }

            foreach (var kpv in remainingProducts)
            {
                expiredProducts.Add(kpv.Key, 0);
            }

            foreach (var import in imports)
            {
                var importDetails = import.ImportDetails;
                foreach (var importDetail in importDetails)
                {
                    var product = await _grpcProductService.GetProduct(importDetail.ProductId);
                    if((DateTime.Now - import.CreatedTime).Days > product.Expiry)
                        if (expiredProducts.ContainsKey(product.Id))
                                expiredProducts[product.Id] += importDetail.Quantity - importDetail.ExportedQuantity;
                }
            }

            var InStockProductDtos = new List<InStockProductDto>();
            foreach (var kpv in remainingProducts)
            {
                var product = await _grpcProductService.GetProduct(kpv.Key);
                var exportedQuantity = 0;
                if (exportProducts.ContainsKey(kpv.Key))
                    exportedQuantity = exportProducts[kpv.Key];
                var expiredQuantity = 0;
                if (expiredProducts.ContainsKey(kpv.Key))
                    expiredQuantity = expiredProducts[kpv.Key];
                InStockProductDtos.Add(new InStockProductDto()
                {
                    Id = kpv.Key,
                    ExportedQuantity = exportedQuantity,
                    RemaningQuantity = remainingProducts[kpv.Key],
                    ExpiredQuantity = expiredQuantity,
                    Name = product.Name,
                    Thumbnail = product.Thumbnail,
                    Unit = product.Unit,
                });
            }

            Console.WriteLine("Import");
            foreach (var kpv in importProducts)
            {
                Console.WriteLine(kpv.Key + ": " + kpv.Value);
            }
            Console.WriteLine("Export");
            foreach (var kpv in exportProducts)
            {
                Console.WriteLine(kpv.Key + ": " + kpv.Value);
            }
            Console.WriteLine("Remaining");
            foreach (var kpv in remainingProducts)
            {
                Console.WriteLine(kpv.Key + ": " + kpv.Value);
            }
            Console.WriteLine("Expired");
            foreach (var kpv in expiredProducts)
            {
                Console.WriteLine(kpv.Key + ": " + kpv.Value);
            }

            return InStockProductDtos;
        }
    }
}
