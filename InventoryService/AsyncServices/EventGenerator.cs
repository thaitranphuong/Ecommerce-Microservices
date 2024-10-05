using InventoryService.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.AsyncServices
{
    public class EventGenerator
    {
        public static string GenerateEvent(EventType eventType) {
            switch (eventType)
            {
                case EventType.CreateImport:
                    return "CreateImport";
                default:
                    return string.Empty;
            }
        } 
    }
}
