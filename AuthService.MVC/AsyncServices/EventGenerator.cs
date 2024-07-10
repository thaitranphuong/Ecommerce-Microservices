using AuthService.MVC.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.AsyncServices
{
    public class EventGenerator
    {
        public static string GenerateEvent(EventType eventType) {
            switch (eventType)
            {
                case EventType.CreateUser:
                    return "CreateUser";
                default:
                    return string.Empty;
            }
        } 
    }
}
