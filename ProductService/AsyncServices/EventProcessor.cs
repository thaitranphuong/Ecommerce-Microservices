using ProductService.Dtos;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Constants;
using ProductService.Services;
using System;
using System.Text.Json;
using System.Collections.Generic;
using ProductService.Repositories;

namespace ProductService.AsyncServices
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IUserService _userService;
        private readonly IProductRepository _productRepository;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            var scope = _scopeFactory.CreateScope();
            _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            _productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        }

        public async void ProcessEvent(string jsonMessage)
        {
            var message = JsonSerializer.Deserialize<AsyncMessageDto<Object>>(jsonMessage);
            var eventType = DetemimeEventType(message.EventType);
            switch (eventType)
            {
                case EventType.CreateUser:
                    UserDto user = JsonSerializer.Deserialize<AsyncMessageDto<UserDto>>(jsonMessage).Data;
                    await _userService.Save(user);
                    Console.WriteLine("Created user");
                    return;
                case EventType.UpdateUser:
                    UserDto userUpdate = JsonSerializer.Deserialize<AsyncMessageDto<UserDto>>(jsonMessage).Data;
                    await _userService.Save(userUpdate);
                    Console.WriteLine("Updated user");
                    return;
                case EventType.CreateImport:
                    List<ProductDto> productDtos = JsonSerializer.Deserialize<AsyncMessageDto<List<ProductDto>>>(jsonMessage).Data;
                    foreach(var productDto in productDtos)
                    {
                        var product = await _productRepository.FindById(productDto.Id);
                        product.Quantity += productDto.Quantity;
                        await _productRepository.SaveChange();
                    }
                    Console.WriteLine("Created import detail");
                    return;
                case EventType.CreateExport:
                    productDtos = JsonSerializer.Deserialize<AsyncMessageDto<List<ProductDto>>>(jsonMessage).Data;
                    foreach (var productDto in productDtos)
                    {
                        var product = await _productRepository.FindById(productDto.Id);
                        product.Quantity -= productDto.Quantity;
                        await _productRepository.SaveChange();
                    }
                    Console.WriteLine("Create export detail");
                    return;
                case EventType.DeleverySuccess:
                    productDtos = JsonSerializer.Deserialize<AsyncMessageDto<List<ProductDto>>>(jsonMessage).Data;
                    foreach (var productDto in productDtos)
                    {
                        var product = await _productRepository.FindById(productDto.Id);
                        product.SoldQuantity += productDto.Quantity;
                        await _productRepository.SaveChange();
                    }
                    Console.WriteLine("Delevery successfully");
                    return;
                default:
                    Console.WriteLine("Undefined");
                    return;
            }
        }

        public EventType DetemimeEventType(string eventType)
        {
            switch (eventType)
            {
                case "CreateUser":
                    return EventType.CreateUser;
                case "UpdateUser":
                    return EventType.UpdateUser;
                case "CreateImport":
                    return EventType.CreateImport;
                case "CreateExport":
                    return EventType.CreateExport;
                case "DeleverySuccess":
                    return EventType.DeleverySuccess;
                default:
                    return EventType.Undefined;
            }
        }
    }
}
