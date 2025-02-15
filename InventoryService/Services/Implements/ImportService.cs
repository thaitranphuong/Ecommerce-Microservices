﻿using AutoMapper;
using InventoryService.AsyncServices;
using InventoryService.Constants;
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
    public class ImportService : IImportService
    {
        private readonly IImportRepository _importRepository;
        private readonly IGrpcProductService _grpcProductService;
        private readonly IGrpcUserService _grpcUserService;
        private readonly IMessageProducer _messageProducer;
        private readonly IMapper _mapper;

        public ImportService(IImportRepository importRepository,
            IMapper mapper, IGrpcProductService grpcProductService,
            IGrpcUserService grpcUserService, IMessageProducer messageProducer)
        {
            _importRepository = importRepository;
            _mapper = mapper;
            _grpcProductService = grpcProductService;
            _grpcUserService = grpcUserService;
            _messageProducer = messageProducer;
        }

        public async Task<bool> Save(ImportDto dto)
        {
            var id = await _importRepository.CreateOne(_mapper.Map<Import>(dto));
            var result = id > 0;
            if(result)
            {
                var productPublishDtos = new List<ProductPublishDto>();
                foreach (var importDetail in dto.ImportDetails)
                {
                    var productPublishDto = new ProductPublishDto { Id = importDetail.ProductId, Quantity = importDetail.Quantity };
                    productPublishDtos.Add(productPublishDto);
                }
                _messageProducer.SendMessage<List<ProductPublishDto>>(EventType.CreateImport, productPublishDtos);
            }
            return result;
        }

        public async Task<ImportDto> FindById(int id)
        {
            var import = await _importRepository.FindById(id);
            var importDto = _mapper.Map<ImportDto>(import);
            var ImportDetailDtos = new List<ImportDetailDto>();
            foreach (var importDetai in import.ImportDetails)
            {
                var importDetailDto = _mapper.Map<ImportDetailDto>(importDetai);
                var product = await _grpcProductService.GetProduct(importDetailDto.ProductId);
                if (product != null)
                {
                    importDetailDto.ProductName = product.Name;
                    importDetailDto.ProductThumbnail = product.Thumbnail;
                    importDetailDto.Unit = product.Unit;
                }
                ImportDetailDtos.Add(importDetailDto);
            }
            importDto.ImportDetails = ImportDetailDtos;
            var user = await _grpcUserService.GetUser(import.UserId);
            if (user != null) importDto.UserName = user.UserName;
            return importDto;
        }

        public async Task<ImportOutput> FindAll(DateTime startTime, DateTime endTime, int page, int limit)
        {
            var imports = await _importRepository.FindAll(startTime, endTime, page, limit);
            var ImportDtos = new List<ImportDto>();
            foreach (var import in imports)
            {
                var ImportDetailDtos = new List<ImportDetailDto>();
                foreach (var importDetai in import.ImportDetails)
                {
                    ImportDetailDtos.Add(_mapper.Map<ImportDetailDto>(importDetai));
                }
                var importDto = _mapper.Map<ImportDto>(import);
                importDto.ImportDetails = ImportDetailDtos;
                var user = await _grpcUserService.GetUser(import.UserId);
                if (user != null) importDto.UserName = user.UserName;
                ImportDtos.Add(importDto);
            }
            ImportOutput output = new ImportOutput();
            output.Page = page;
            output.TotalPage = (int)Math.Ceiling((double)(await _importRepository.FindAllByDate(startTime, endTime)).Count / limit);
            output.ListResult = ImportDtos;
            return output;
        }

        public async Task<bool> UpdateImportDetail(List<ImportDetailDto> importDetailDtos)
        {
            foreach (var dto in importDetailDtos)
            {
                var importDetail = await _importRepository.FindImportDetailById(dto.Id);
                importDetail.ExportedQuantity += dto.ExportedQuantity;
                await _importRepository.SaveChange();
            }

            return true;
        }

        public async Task<List<ItemLineDto>> GetAllItemLines(int warehouseId)
        {
            var imports = await _importRepository.FindByWarehouseId(warehouseId);
            var itemLineDtos = new List<ItemLineDto>();
            foreach (var import in imports)
            {
                foreach (var importDetail in import.ImportDetails)
                {
                    var product = await _grpcProductService.GetProduct(importDetail.ProductId);
                    bool isExpired = (DateTime.Now - import.CreatedTime).Days > product.Expiry;
                    var status = isExpired ? "HẾT HẠN" : "CÒN HẠN";
                    var itemLineDto = new ItemLineDto()
                    {
                        Id = importDetail.Id,
                        ImportDetailId = importDetail.Id,
                        ProductId = importDetail.ProductId,
                        ImportId = importDetail.ImportId,
                        RemainingQuantity = importDetail.Quantity - importDetail.ExportedQuantity,
                        IsExpired = isExpired,
                        Unit = product.Unit,
                        Name = "Mã lô (phiếu nhập): " + import.Id + "  |  " + product.Name + "  |  " + importDetail.Position + " [" + status + "]",
                    };
                    itemLineDtos.Add(itemLineDto);
                }

            }
            return itemLineDtos;
        }
    }
}
