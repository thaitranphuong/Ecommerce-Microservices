using AutoMapper;
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
    public class ExportService : IExportService
    {
        private readonly IExportRepository _exportRepository;
        private readonly IGrpcProductService _grpcProductService;
        private readonly IGrpcUserService _grpcUserService;
        private readonly IMessageProducer _messageProducer;
        private readonly IMapper _mapper;

        public ExportService(IExportRepository exportRepository,
            IMapper mapper, IGrpcProductService grpcProductService,
            IGrpcUserService grpcUserService, IMessageProducer messageProducer)
        {
            _exportRepository = exportRepository;
            _mapper = mapper;
            _grpcProductService = grpcProductService;
            _grpcUserService = grpcUserService;
            _messageProducer = messageProducer;
        }

        public async Task<bool> Save(ExportDto dto)
        {
            var id = await _exportRepository.CreateOne(_mapper.Map<Export>(dto));
            var result = id > 0;
            if(result)
            {
                var productPublishDtos = new List<ProductPublishDto>();
                foreach (var exportDetail in dto.ExportDetails)
                {
                    var productPublishDto = new ProductPublishDto { Id = exportDetail.ProductId, Quantity = exportDetail.Quantity };
                    productPublishDtos.Add(productPublishDto);
                }
                _messageProducer.SendMessage<List<ProductPublishDto>>(EventType.CreateExport, productPublishDtos);
            }
            return result;
        }

        public async Task<ExportDto> FindById(int id)
        {
            var export = await _exportRepository.FindById(id);
            var exportDto = _mapper.Map<ExportDto>(export);
            var ExportDetailDtos = new List<ExportDetailDto>();
            foreach (var exportDetai in export.ExportDetails)
            {
                var exportDetailDto = _mapper.Map<ExportDetailDto>(exportDetai);
                var product = await _grpcProductService.GetProduct(exportDetailDto.ProductId);
                if (product != null)
                {
                    exportDetailDto.ProductName = product.Name;
                    exportDetailDto.ProductThumbnail = product.Thumbnail;
                    exportDetailDto.Unit = product.Unit;
                }
                ExportDetailDtos.Add(exportDetailDto);
            }
            exportDto.ExportDetails = ExportDetailDtos;
            var user = await _grpcUserService.GetUser(export.UserId);
            if (user != null) exportDto.UserName = user.UserName;
            return exportDto;
        }

        public async Task<ExportOutput> FindAll(DateTime startTime, DateTime endTime, int page, int limit)
        {
            var exports = await _exportRepository.FindAll(startTime, endTime, page, limit);
            var ExportDtos = new List<ExportDto>();
            foreach (var export in exports)
            {
                var ExportDetailDtos = new List<ExportDetailDto>();
                foreach (var exportDetai in export.ExportDetails)
                {
                    ExportDetailDtos.Add(_mapper.Map<ExportDetailDto>(exportDetai));
                }
                var exportDto = _mapper.Map<ExportDto>(export);
                exportDto.ExportDetails = ExportDetailDtos;
                var user = await _grpcUserService.GetUser(export.UserId);
                if (user != null) exportDto.UserName = user.UserName;
                ExportDtos.Add(exportDto);
            }
            ExportOutput output = new ExportOutput();
            output.Page = page;
            output.TotalPage = (int)Math.Ceiling((double)(await _exportRepository.FindAllByDate(startTime, endTime)).Count / limit);
            output.ListResult = ExportDtos;
            return output;
        }
    }
}
