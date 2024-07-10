using AuthService.MVC.Constants;
using AuthService.MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Helpers
{
    public class TempDataGenerator
    {
        public static string Generate(string notificationType, string message)
        {
            return JsonConvert.SerializeObject(new ToastifyModel()
            {
                Status = notificationType,
                Message = message
            });
        }
    }
}
