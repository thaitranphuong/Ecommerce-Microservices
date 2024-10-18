using OrderService.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.AsyncServices
{
    public class EventGenerator
    {
        public static string GenerateEvent(EventType eventType) {
            switch (eventType)
            {
                case EventType.RemoveCartItem:
                    return "RemoveCartItem";
                case EventType.ReduceProductQuantity:
                    return "ReduceProductQuantity";
                default:
                    return string.Empty;
            }
        } 
    }
}
