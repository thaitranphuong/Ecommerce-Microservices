using ProductService.Dtos;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Constants;
using ProductService.Services;
using System;
using System.Text.Json;

namespace ProductService.AsyncServices
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IUserService _userService;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            var scope = _scopeFactory.CreateScope();
            _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        }

        public void ProcessEvent(string jsonMessage)
        {
            var message = JsonSerializer.Deserialize<AsyncMessageDto<Object>>(jsonMessage);
            var eventType = DetemimeEventType(message.EventType);
            switch (eventType)
            {
                case EventType.CreateUser:
                    UserDto user = JsonSerializer.Deserialize<AsyncMessageDto<UserDto>>(jsonMessage).Data;
                    _userService.Save(user);
                    Console.WriteLine("Created user");
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
                default:
                    return EventType.Undefined;
            }
        }
    }
}
