using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Dtos
{
    public class UserPublishDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
    }
}
