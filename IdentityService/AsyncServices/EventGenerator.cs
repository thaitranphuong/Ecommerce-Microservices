using IdentityService.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.AsyncServices
{
    public class EventGenerator
    {
        public static string GenerateEvent(EventType eventType) {
            switch (eventType)
            {
                case EventType.CreateUser:
                    return "CreateUser";
                case EventType.UpdateUser:
                    return "UpdateUser";
                default:
                    return string.Empty;
            }
        } 
    }
}
