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

        public async Task<List<WarehouseDto>> FindAllToExport(int productId, int productQuantity)
        {
            var importDetails = await _importRepository.FindAllByProductId(productId);
            var warehouseHadImportQuantity = new Dictionary<int, int>();
            var warehouseHadExportQuantity = new Dictionary<int, int>();
            var warehouseRemainingQuantity = new Dictionary<int, int>();

            foreach (var importDetail in importDetails)
            {
                var warehouseId = importDetail.Import.WarehouseId;
                if (!warehouseHadImportQuantity.ContainsKey(warehouseId))
                {
                    warehouseHadImportQuantity.Add(warehouseId, importDetail.Quantity);
                }
                else
                {
                    warehouseHadImportQuantity[warehouseId] += importDetail.Quantity;
                }
            }

            //var orderDetails = (await _grpcOrderDetailService.GetOrderDetail(0, productId, 0)).OrderDetails;
            //foreach (var orderDetail in orderDetails)
            //{
            //    var warehouseId = orderDetail.WarehouseId;
            //    if (warehouseId != 0)
            //    {
            //        if (!warehouseHadExportQuantity.ContainsKey(warehouseId))
            //        {
            //            warehouseHadExportQuantity.Add(warehouseId, orderDetail.Quantity);
            //        }
            //        else
            //        {
            //            warehouseHadExportQuantity[warehouseId] += orderDetail.Quantity;
            //        }
            //    }
            //}

            // Duyệt qua tất cả các kho trong dictionary nhập hàng
            foreach (var import in warehouseHadImportQuantity)
            {
                int warehouseId = import.Key;
                int importQuantity = import.Value;

                if (warehouseHadExportQuantity.TryGetValue(warehouseId, out int exportQuantity))
                {
                    int remainingQuantity = importQuantity - exportQuantity;
                    warehouseRemainingQuantity[warehouseId] = remainingQuantity;
                }
                else
                {
                    warehouseRemainingQuantity[warehouseId] = importQuantity;
                }
            }

            // Duyệt qua các kho chỉ có trong danh sách xuất hàng mà không có trong danh sách nhập hàng
            foreach (var export in warehouseHadExportQuantity)
            {
                int warehouseId = export.Key;
                if (!warehouseHadImportQuantity.ContainsKey(warehouseId))
                {
                    warehouseRemainingQuantity[warehouseId] = -export.Value;
                }
            }

            var qualifiedWarehouseToExports = new List<WarehouseDto>();
            foreach (var kpv in warehouseRemainingQuantity)
            {
                if(kpv.Value >= productQuantity)
                {
                    var warehouse = await _warehouseRepository.FindById(kpv.Key);
                    if (warehouse != null)
                    {
                        var warehouseDto = new WarehouseDto()
                        {
                            Id = warehouse.Id,
                            Name = warehouse.Name,
                            Address = warehouse.Address,
                            InStockProducts = new List<InStockProductDto>(),
                        };
                        warehouseDto.InStockProducts.Add(new InStockProductDto()
                        {
                            Id = productId,
                            RemaningQuantity = warehouseRemainingQuantity[kpv.Key],
                        });
                        qualifiedWarehouseToExports.Add(warehouseDto);
                    }
                }
            }

            return qualifiedWarehouseToExports;
        }

        public async Task<List<InStockProductDto>> FindAllInstock(int id)
        {
            var imports = await _importRepository.FindByWarehouseId(id);
            var importProducts = new Dictionary<int, int>();
            var exportProducts = new Dictionary<int, int>();
            var remainingProducts = new Dictionary<int, int>();


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
                }
            }

            //var orderDetails = (await _grpcOrderDetailService.GetOrderDetail(0, 0, id)).OrderDetails;
            //foreach (var orderDetail in orderDetails)
            //{
            //    var productId = orderDetail.ProductId;
            //    if (!exportProducts.ContainsKey(productId))
            //    {
            //        exportProducts.Add(productId, orderDetail.Quantity);
            //    }
            //    else
            //    {
            //        exportProducts[productId] += orderDetail.Quantity;
            //    }
            //}

            // Duyệt qua tất cả các kho trong dictionary nhập hàng
            foreach (var import in importProducts)
            {
                int productId = import.Key;
                int importQuantity = import.Value;

                if (exportProducts.TryGetValue(productId, out int exportQuantity))
                {
                    int remainingQuantity = importQuantity - exportQuantity;
                    remainingProducts[productId] = remainingQuantity;
                }
                else
                {
                    remainingProducts[productId] = importQuantity;
                }
            }

            // Duyệt qua các kho chỉ có trong danh sách xuất hàng mà không có trong danh sách nhập hàng
            foreach (var export in exportProducts)
            {
                int productId = export.Key;
                if (!importProducts.ContainsKey(productId))
                {
                    remainingProducts[productId] = -export.Value;
                }
            }

            var InStockProductDtos = new List<InStockProductDto>();
            foreach (var kpv in remainingProducts)
            {
                var product = await _grpcProductService.GetProduct(kpv.Key);
                var exportedQuantity = 0;
                if (exportProducts.ContainsKey(kpv.Key))
                    exportedQuantity = exportProducts[kpv.Key];
                InStockProductDtos.Add(new InStockProductDto()
                {
                    Id = kpv.Key,
                    ExportedQuantity = exportedQuantity,
                    RemaningQuantity = remainingProducts[kpv.Key],
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

            return InStockProductDtos;
        }
    }
}
