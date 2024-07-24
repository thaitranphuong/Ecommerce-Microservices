using CartService.Dtos;
using Microsoft.Extensions.DependencyInjection;
using CartService.Constants;
using CartService.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CartService.AsyncServices
{
    public class EventProcessor : IEventProcessor
    {
        private readonly ICartItemService _cartItemService;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            var scope = _scopeFactory.CreateScope();
            _cartItemService = scope.ServiceProvider.GetRequiredService<ICartItemService>();
        }

        public async Task ProcessEvent(string jsonMessage)
        {
            var message = JsonSerializer.Deserialize<AsyncMessageDto<Object>>(jsonMessage);
            var eventType = DetemimeEventType(message.EventType);
            switch (eventType)
            {
                case EventType.RemoveCartItem:
                    CartItemDto cartItem = JsonSerializer.Deserialize<AsyncMessageDto<CartItemDto>>(jsonMessage).Data;
                    await _cartItemService.Delete(cartItem.UserId, cartItem.ProductId);
                    Console.WriteLine("Deleted cart item");
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
                case "RemoveCartItem":
                    return EventType.RemoveCartItem;
                default:
                    return EventType.Undefined;
            }
        }
    }
}
